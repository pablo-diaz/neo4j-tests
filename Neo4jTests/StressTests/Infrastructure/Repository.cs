using System.Linq;

using StressTests.Domain;
using System.Collections.Generic;

using Neo4jClient;
using System;
using System.Diagnostics;

namespace StressTests.Infrastructure
{
    // https://github.com/Readify/Neo4jClient/wiki/cypher-examples
    public class Repository
    {
        private readonly IGraphClient _neo4jClient;
        private readonly Stopwatch _stopWatch = new Stopwatch();

        public Repository(IGraphClient neo4jClient)
        {
            this._neo4jClient = neo4jClient;
        }

        public void Save(IEnumerable<Company> companies, Action<string> logger, Func<bool> continueWithNextCompanyFn)
        {
            foreach(var company in companies)
            {
                logger($"Starting to save company {company}");
                this._stopWatch.Start();

                Save(company, logger);

                this._stopWatch.Stop();
                logger($"Finished saving company {company}. Total time: {this._stopWatch.Elapsed}");

                if (!continueWithNextCompanyFn())
                    break;
            }
        }

        #region Helpers

        private void Save(Company company, Action<string> logger)
        {
            SaveCompanyInfo(company.MapToDTO());
            var users = company.Users.ToList();
            SaveUsers(users, company);
        }

        private void SaveCompanyInfo(CompanyDTO company)
        {
            this._neo4jClient
                .Cypher
                .Create("(c:Company {newCompany})")
                .WithParam("newCompany", company)
                .ExecuteWithoutResults();
        }

        private void SaveUsers(List<User> users, Company company)
        {
            users.Select(u => u.MapToDTO())
                .ToList()
                .ForEach(u => SaveUserInfo(u));

            CorrelateUsersWithCompany(company, users);
            CorrelateUsersWithManagers(users);

            company.HeadCoaches
                .ToList()
                .ForEach(u => SaveChallengesForUser(u));

            company.ContentAuthors
                .ToList()
                .ForEach(u => SavePresentationsForUser(u));

            company.CourseAuthors
                .ToList()
                .ForEach(u => SaveCoursesForUser(u));
        }

        private void SaveUserInfo(UserDTO user)
        {
            this._neo4jClient
                .Cypher
                .Create("(u:User {newUser})")
                .WithParam("newUser", user)
                .ExecuteWithoutResults();
        }

        private void CorrelateUsersWithCompany(Company company, List<User> users)
        {
            users.ForEach(user => this._neo4jClient
                .Cypher
                .Match("(c:Company)", "(u:User)")
                .Where<CompanyDTO>(c => c.id == company.Id)
                .AndWhere<UserDTO>(u => u.id == user.Id)
                .Create("(u)-[:WorksAt]->(c)")
                .ExecuteWithoutResults());
        }

        private void CorrelateUsersWithManagers(List<User> users)
        {
            users.ForEach(u => CorrelateUserWithManager(u, u.Manager));
        }

        private void CorrelateUserWithManager(User user, User manager)
        {
            if (manager == null)
                return;

            this._neo4jClient
                .Cypher
                .Match("(m:User)", "(u:User)")
                .Where<UserDTO>(m => m.id == manager.Id)
                .AndWhere<UserDTO>(u => u.id == user.Id)
                .Create("(m)-[:Manages]->(u)")
                .ExecuteWithoutResults();
        }

        private void SaveChallengesForUser(User user)
        {
            user.AuthorOfChallenges
                .ToList()
                .ForEach(ch => {
                    SaveChallengeInfo(ch.MapToDTO());
                    CorrelateChallengeWithItsCreator(ch, user);
                    CorrelateChallengeWithUsers(ch, ch.Participants.ToList(), "ParticipatesIn");
                    CorrelateChallengeWithUsers(ch, ch.Reviewers.ToList(), "Reviews");
                });
        }

        private void SaveChallengeInfo(ChallengeDTO challenge)
        {
            this._neo4jClient
                .Cypher
                .Create("(c:Challenge {newChallenge})")
                .WithParam("newChallenge", challenge)
                .ExecuteWithoutResults();
        }

        private void CorrelateChallengeWithItsCreator(Challenge challenge, User creator)
        {
            this._neo4jClient
                .Cypher
                .Match("(c:Challenge)", "(u:User)")
                .Where<ChallengeDTO>(c => c.id == challenge.Id)
                .AndWhere<UserDTO>(u => u.id == creator.Id)
                .Create("(u)-[:HasCreated]->(c)")
                .ExecuteWithoutResults();
        }

        private void CorrelateChallengeWithUsers(Challenge challenge, List<User> usersToCorrelate, string nameOfRelationship)
        {
            usersToCorrelate.ForEach(user => this._neo4jClient
                .Cypher
                .Match("(c:Challenge)", "(u:User)")
                .Where<ChallengeDTO>(c => c.id == challenge.Id)
                .AndWhere<UserDTO>(u => u.id == user.Id)
                .Create($"(u)-[:{nameOfRelationship}]->(c)")
                .ExecuteWithoutResults()
            );
        }

        private void SavePresentationsForUser(User user)
        {
            user.AuthorOfContent
                .ToList()
                .ForEach(pr => {
                    SavePresentationInfo(pr.MapToDTO());
                    CorrelatePresentationWithItsCreator(pr, user);
                    SaveSlidesInfo(pr.Slides);
                    CorrelateSlidesWithPresentation(pr.Slides, pr);
                });
        }

        private void SavePresentationInfo(PresentationDTO presentation)
        {
            this._neo4jClient
                .Cypher
                .Create("(p:Presentation {newPresentation})")
                .WithParam("newPresentation", presentation)
                .ExecuteWithoutResults();
        }

        private void CorrelatePresentationWithItsCreator(Presentation presentation, User author)
        {
            this._neo4jClient
                .Cypher
                .Match("(p:Presentation)", "(u:User)")
                .Where<PresentationDTO>(p => p.id == presentation.Id)
                .AndWhere<UserDTO>(u => u.id == author.Id)
                .Create("(u)-[:HasCreated]->(p)")
                .ExecuteWithoutResults();
        }

        private void SaveSlidesInfo(IEnumerable<Slide> slides)
        {
            slides.Select(s => s.MapToDTO())
                .ToList()
                .ForEach(slide => this._neo4jClient
                .Cypher
                .Create("(s:Slide {newSlide})")
                .WithParam("newSlide", slide)
                .ExecuteWithoutResults());
        }

        private void CorrelateSlidesWithPresentation(IEnumerable<Slide> slides, Presentation presentation)
        {
            slides.ToList()
                .ForEach(slide => this._neo4jClient
                .Cypher
                .Match("(p:Presentation)", "(s:Slide)")
                .Where<PresentationDTO>(p => p.id == presentation.Id)
                .AndWhere<SlideDTO>(s => s.id == slide.Id)
                .Create("(s)-[:IsPartOf]->(p)")
                .ExecuteWithoutResults());
        }

        private void SaveCoursesForUser(User user)
        {
            user.AuthorOfCourses
                .ToList()
                .ForEach(course => {
                    SaveCourseInfo(course.MapToDTO());
                    CorrelateCourseWithItsCreator(course, user);
                    SavePresentationInfo(course.BasedOn.MapToDTO());
                    CorrelateCourseWithItsUndelryingPresentation(course, course.BasedOn);
                    SaveSlidesInfo(course.BasedOn.Slides);
                    CorrelateSlidesWithPresentation(course.BasedOn.Slides, course.BasedOn);
                });
        }

        private void SaveCourseInfo(CourseDTO course)
        {
            this._neo4jClient
                .Cypher
                .Create("(c:Course {newCourse})")
                .WithParam("newCourse", course)
                .ExecuteWithoutResults();
        }

        private void CorrelateCourseWithItsCreator(Course course, User author)
        {
            this._neo4jClient
                .Cypher
                .Match("(c:Course)", "(u:User)")
                .Where<CourseDTO>(c => c.id == course.Id)
                .AndWhere<UserDTO>(u => u.id == author.Id)
                .Create("(u)-[:HasCreated]->(c)")
                .ExecuteWithoutResults();
        }

        private void CorrelateCourseWithItsUndelryingPresentation(Course course, Presentation underlyingPresentation)
        {
            this._neo4jClient
                .Cypher
                .Match("(c:Course)", "(p:Presentation)")
                .Where<CourseDTO>(c => c.id == course.Id)
                .AndWhere<PresentationDTO>(p => p.id == underlyingPresentation.Id)
                .Create("(c)-[:BasedOn]->(p)")
                .ExecuteWithoutResults();
        }

        #endregion
    }
}
