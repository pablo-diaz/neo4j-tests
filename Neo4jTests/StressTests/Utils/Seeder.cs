using System.Linq;
using StressTests.Domain;
using System.Collections.Generic;
using System;

namespace StressTests.Utils
{
    public static class Seeder
    {
        private static Random _randomGenerator = new Random();

        public static IEnumerable<Company> CreateCompanies()
        {
            while(true)
                yield return CreateCompany();
        } 

        #region Private helpers

        private static Company CreateCompany()
        {
            var numberOfUsersToCreate = _randomGenerator.Next(20, 40);
            var users = CreateUsers(numberOfUsersToCreate);

            var numberOfHeadCoachesToCreate = _randomGenerator.Next(1, Math.Min(5, users.Count));
            var headCoaches = CreateHeadCoaches(numberOfHeadCoachesToCreate, users, 10);

            var numberOfContentAuthorsToCreate = _randomGenerator.Next(1, Math.Min(10, users.Count));
            var contentAuthors = CreateContentAuthors(numberOfContentAuthorsToCreate, users, 20);

            var numberOfCourseAuthorsToCreate = _randomGenerator.Next(1, Math.Min(10, users.Count));
            var courseAuthors = CreateCourseAuthors(numberOfCourseAuthorsToCreate, users, 20);

            var companyId = IdProvider.GetNextCompanyId();
            return new Company(companyId, companyId.GetName("Company"), users, headCoaches, contentAuthors, courseAuthors);
        }

        private static List<User> CreateUsers(int numberOfUsersToCreate)
        {
            return Enumerable.Range(1, numberOfUsersToCreate)
                .Select(_ => CreateUser())
                .ToList()
                .AssignManagers();
        }

        private static User CreateUser()
        {
            var userId = IdProvider.GetNextUserId();
            return new User(userId, userId.GetName("Firstname"), userId.GetName("Lastname"), userId.GetName("Login"), null, null, null, null);
        }

        private static List<User> AssignManagers(this List<User> users)
        {
            var newUserList = new List<User>();

            var generalManager = users.Take(1).FirstOrDefault();
            if (generalManager != null)
                newUserList.Add(generalManager);

            var firstLayerOfManagers = users
                .Skip(1)
                .Take(4)
                .ToList()
                .AssignManagerToUsers(generalManager);
            newUserList.AddRange(firstLayerOfManagers);

            var secondLayerOfManagers = new List<User>();
            var secondLayerOfManagersBlockSize = 10;
            var constantSkipCount = 1 + firstLayerOfManagers.Count;
            for (int i = 0; i < firstLayerOfManagers.Count; i++)
            {
                var manager = firstLayerOfManagers[i];
                var skipCount = constantSkipCount + (i * secondLayerOfManagersBlockSize);
                var otherUsers = users
                    .Skip(skipCount)
                    .Take(secondLayerOfManagersBlockSize)
                    .ToList()
                    .AssignManagerToUsers(manager);
                secondLayerOfManagers.AddRange(otherUsers);
            }
            newUserList.AddRange(secondLayerOfManagers);

            var thirdLayerOfManagers = new List<User>();
            var thirdLayerOfManagersBlockSize = 30;
            constantSkipCount += secondLayerOfManagers.Count;
            for (int i = 0; i < secondLayerOfManagers.Count; i++)
            {
                var manager = secondLayerOfManagers[i];
                var skipCount = constantSkipCount + (i * thirdLayerOfManagersBlockSize);
                var otherUsers = users
                    .Skip(skipCount)
                    .Take(thirdLayerOfManagersBlockSize)
                    .ToList()
                    .AssignManagerToUsers(manager);
                thirdLayerOfManagers.AddRange(otherUsers);
            }
            newUserList.AddRange(thirdLayerOfManagers);

            var fourthLayerOfManagers = new List<User>();
            var fourthLayerOfManagersBlockSize = 50;
            constantSkipCount += thirdLayerOfManagers.Count;
            for (int i = 0; i < thirdLayerOfManagers.Count; i++)
            {
                var manager = thirdLayerOfManagers[i];
                var skipCount = constantSkipCount + (i * fourthLayerOfManagersBlockSize);
                var otherUsers = users
                    .Skip(skipCount)
                    .Take(fourthLayerOfManagersBlockSize)
                    .ToList()
                    .AssignManagerToUsers(manager);
                fourthLayerOfManagers.AddRange(otherUsers);
            }
            newUserList.AddRange(fourthLayerOfManagers);

            return newUserList;
        }

        private static List<User> AssignManagerToUsers(this List<User> users, User manager)
        {
            var newUserList = new List<User>();

            users.Select(u => u.AssignManager(manager))
                .ToList()
                .ForEach(u => newUserList.Add(u));

            return newUserList;
        }

        private static List<User> GetRandomUsers(this List<User> users, int count)
        {
            if (users.Count < count)
                throw new ArgumentException($"Requested to generate {count} users, but provided only {users.Count}");
            var newUserList = new List<User>();
            var positionsTaken = new Dictionary<int, bool>();

            while(newUserList.Count < count)
            {
                int randomPosition = _randomGenerator.Next(count);
                if(!positionsTaken.ContainsKey(randomPosition))
                {
                    newUserList.Add(users[randomPosition]);
                    positionsTaken[randomPosition] = true;
                }
            }

            return newUserList;
        }

        private static List<User> CreateHeadCoaches(int numberOfHeadCoachesToCreate, List<User> basedOnUsers, int maxChallengesPerHeadCoach)
        {
            return basedOnUsers
                .GetRandomUsers(numberOfHeadCoachesToCreate)
                .Select(u => CreateChallengesForUser(u, basedOnUsers, maxChallengesPerHeadCoach))
                .ToList();
        }

        private static User CreateChallengesForUser(User user, List<User> usersToGrabParticipantsAndReviewersFrom, int maxChallengesPerHeadCoach)
        {
            var numberOfChallengesToCreate = _randomGenerator.Next(5, maxChallengesPerHeadCoach);
            var newChallenges = Enumerable.Range(1, numberOfChallengesToCreate)
                .Select(_ => CreateChallenge(usersToGrabParticipantsAndReviewersFrom))
                .ToList();

            return user.AddChallenges(newChallenges);
        }

        private static Challenge CreateChallenge(List<User> usersToGrabParticipantsAndReviewersFrom)
        {
            var numberOfParticipantsToCreate = _randomGenerator.Next(Math.Min(usersToGrabParticipantsAndReviewersFrom.Count, 5), Math.Min(usersToGrabParticipantsAndReviewersFrom.Count, 10));
            var numberOfReviewersToCreate = _randomGenerator.Next(1, Math.Min(usersToGrabParticipantsAndReviewersFrom.Count, 5));
            var participants = usersToGrabParticipantsAndReviewersFrom.GetRandomUsers(numberOfParticipantsToCreate);
            var reviewers = usersToGrabParticipantsAndReviewersFrom.GetRandomUsers(numberOfReviewersToCreate);
            var challengeId = IdProvider.GetNextChallengeId();
            return new Challenge(challengeId, challengeId.GetName("Challenge"), participants, reviewers);
        }

        private static List<User> CreateContentAuthors(int numberOfContentAuthorsToCreate, List<User> basedOnUsers, int maxPresentationsPerAuthor)
        {
            return basedOnUsers
                .GetRandomUsers(numberOfContentAuthorsToCreate)
                .Select(u => CreatePresentationsForUser(u, maxPresentationsPerAuthor))
                .ToList();
        }

        private static User CreatePresentationsForUser(User user, int maxPresentationsPerAuthor)
        {
            var numberOfPresentationsToCreate = _randomGenerator.Next(5, maxPresentationsPerAuthor);
            var newPresentations = Enumerable.Range(1, numberOfPresentationsToCreate)
                .Select(_ => CreatePresentation())
                .ToList();

            return user.AddPresentations(newPresentations);
        }

        private static Presentation CreatePresentation()
        {
            int numberOfSlidesToCreate = _randomGenerator.Next(1, 10);
            var slides = Enumerable.Range(1, numberOfSlidesToCreate)
                .Select(order => CreateSlide(order))
                .ToList();

            var presentationId = IdProvider.GetNextPresentationId();
            return new Presentation(presentationId, presentationId.GetName("Presentation"), slides);
        }

        private static Slide CreateSlide(int slideOrder)
        {
            var slideId = IdProvider.GetNextSlideId();
            return new Slide(slideId, slideOrder.GetName("Slide"), slideOrder);
        }

        private static List<User> CreateCourseAuthors(int numberOfCourseAuthorsToCreate, List<User> basedOnUsers, int maxCoursesPerAuthor)
        {
            return basedOnUsers
                .GetRandomUsers(numberOfCourseAuthorsToCreate)
                .Select(u => CreateCoursesForUser(u, maxCoursesPerAuthor))
                .ToList();
        }

        private static User CreateCoursesForUser(User user, int maxCoursesPerAuthor)
        {
            var numberOfCoursesToCreate = _randomGenerator.Next(5, maxCoursesPerAuthor);
            var newCourses = Enumerable.Range(1, numberOfCoursesToCreate)
                .Select(_ => CreateCourse())
                .ToList();

            return user.AddCourses(newCourses);
        }

        private static Course CreateCourse()
        {
            var basedOnPresentation = CreatePresentation();
            var courseId = IdProvider.GetNextCourseId();
            return new Course(courseId, courseId.GetName("Course"), basedOnPresentation);
        }

        #endregion

    }
}
