namespace StressTests.Utils
{
    public static class IdProvider
    {
        private static int _currentUserId = 1;
        private static int _currentCompanyId = 1;
        private static int _currentPresentationId = 1;
        private static int _currentSlideId = 1;
        private static int _currentCourseId = 1;
        private static int _currentChallengeId = 1;

        public static int GetNextUserId() => _currentUserId++;
        public static int GetNextCompanyId() => _currentCompanyId++;
        public static int GetNextPresentationId() => _currentPresentationId++;
        public static int GetNextSlideId() => _currentSlideId++;
        public static int GetNextCourseId() => _currentCourseId++;
        public static int GetNextChallengeId() => _currentChallengeId++;
    }
}