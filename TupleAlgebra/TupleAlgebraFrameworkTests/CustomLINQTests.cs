using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TupleAlgebraFrameworkTests.DataModels;
using System.Linq.Expressions;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Diagnostics;
using LINQProvider;
using LINQProvider.QueryPipelineInfrastructure;
using TupleAlgebraFrameworkTests.CustomLinqTests;

[assembly: InternalsVisibleTo("LINQProvider")]

namespace TupleAlgebraFrameworkTests
{
    [TestClass]
    public class CustomLINQTests
    {
        [TestMethod]
        public void AggregateQuery()
        {
            MockQueryable source = new MockQueryable();
            int query = source.Aggregate(0, (acc, data) => acc + data);

            int sum = MockQueryable.DataSource.Aggregate(0, (acc, data) => acc + data);

            Assert.IsTrue(sum == query);
        }

        [TestMethod]
        public void AllQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.All(QueryContent.GreaterThan0);

            bool allGreaterThan0Predefined = MockQueryable.DataSource.All(data => data > 0),
                 allGreaterThan0 = query;

            Assert.IsTrue(allGreaterThan0Predefined == allGreaterThan0);
        }

        [TestMethod]
        public void AnyQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Any(QueryContent.GreaterThan5);

            bool anyGreaterThan5Predefined = MockQueryable.DataSource.Any(data => data > 5),
                 anyGreaterThan5 = query;

            Assert.IsTrue(anyGreaterThan5Predefined == anyGreaterThan5);
        }

        [TestMethod]
        public void BufSelectQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .BufSelect<int, bool>(QueryContent.IsOdd).AsMockQueryable();

            IEnumerable<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Select(data => (data & 1) == 1),
                      bufSelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, bufSelectIsOdd));
        }

        [TestMethod]
        public void TakeBufSelectTakeQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Take(QueryContent.Number1000).AsMockQueryable()
                .BufSelect<int, bool>(QueryContent.IsOdd).AsMockQueryable()
                .Take(QueryContent.Number10).AsMockQueryable();

            IEnumerable<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Take(1000).Select(data => (data & 1) == 1).Take(10),
                      bufSelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, bufSelectIsOdd));
        }

        [TestMethod]
        public void TakeBufSelectTakeQuery2()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Take(QueryContent.Number10).AsMockQueryable()
                .BufSelect<int, bool>(QueryContent.IsOdd).AsMockQueryable()
                .Take(QueryContent.Number1000).AsMockQueryable();

            IEnumerable<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Take(10).Select(data => (data & 1) == 1).Take(1000),
                      bufSelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, bufSelectIsOdd));
        }

        [TestMethod]
        public void TakeBufSelectToArrayQuery()
        {
            MockQueryable source = new MockQueryable();
            bool[] query = source
                .Take(QueryContent.Number10).AsMockQueryable()
                .BufSelect<int, bool>(QueryContent.IsOdd).AsMockQueryable()
                .ToArray();

            IEnumerable<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Take(10).Select(data => (data & 1) == 1).ToArray(),
                      bufSelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, bufSelectIsOdd));
        }

        [TestMethod]
        public void TakeBufSelectTakeBufSelectQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Take(QueryContent.Number1000).AsMockQueryable()
                .BufSelect<int, int>(QueryContent.Pow2).AsMockQueryable()
                .Take(QueryContent.Number10).AsMockQueryable()
                .BufSelect<int, bool>(QueryContent.IsOdd).AsMockQueryable();

            IEnumerable<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Take(1000).Select(data => data ^ 2)
                                        .Take(10).Select(data => (data & 1) == 1),
                      bufSelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, bufSelectIsOdd));
        }

        [TestMethod]
        public void TakeBufSelectBufSelectTakeQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Take(QueryContent.Number1000).AsMockQueryable()
                .BufSelect<int, int>(QueryContent.Pow2).AsMockQueryable()
                .BufSelect<int, bool>(QueryContent.IsOdd).AsMockQueryable()
                .Take(QueryContent.Number10).AsMockQueryable();

            IEnumerable<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Take(1000).Select(data => data ^ 2)
                                        .Select(data => (data & 1) == 1).Take(10),
                      bufSelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, bufSelectIsOdd));
        }

        [TestMethod]
        public void BufSelectBufSelectBufSelectBufSelectQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .BufSelect<int, int>(QueryContent.All).AsMockQueryable()
                .BufSelect<int, int>(QueryContent.All).AsMockQueryable()
                .BufSelect<int, int>(QueryContent.All).AsMockQueryable()
                .BufSelect<int, int>(QueryContent.All).AsMockQueryable();

            IEnumerable<int> selectIsOddPredefined =
                MockQueryable.DataSource.Select(x => x),
                      bufSelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, bufSelectIsOdd));
        }

        [TestMethod]
        public void ContainsQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Contains(QueryContent.Number10);

            bool contains10Predefined = MockQueryable.DataSource.Contains(10),
                 contains10 = query;

            Assert.IsTrue(contains10Predefined == contains10);
        }

        [TestMethod]
        public void CountQuery()
        {
            MockQueryable source = new MockQueryable();
            int query = source.Count();

            int countPredefined = MockQueryable.DataSource.Count(),
                count = query;

            Assert.IsTrue(countPredefined == count);
        }

        [TestMethod]
        public void CountByFilterQuery()
        {
            MockQueryable source = new MockQueryable();
            int query = source.Count(QueryContent.GreaterThan5);
        
            int countOfGreaterThan5Predefined = MockQueryable.DataSource.Count(data => data > 5),
                countOfGreaterThan5 = query;

            Assert.IsTrue(countOfGreaterThan5Predefined == countOfGreaterThan5);
        }

        [TestMethod]
        public void FirstQuery()
        {
            MockQueryable source = new MockQueryable();
            int query = source.First(QueryContent.GreaterThan7);

           int firstGreaterThan7Predefined = MockQueryable.DataSource.First(data => data > 7),
               firstGreaterThan7 = query;

            Assert.IsTrue(firstGreaterThan7Predefined == firstGreaterThan7);
        }

        [TestMethod]
        public void FirstWhereQuery()
        {
            MultidimensionalMockQueryable source = new MultidimensionalMockQueryable();
            GenericMockQueryable<int> query = source.First().Where(x => x < 5).AsMockQueryable();

            IEnumerable<int> firstGreaterThan7Predefined = MultidimensionalMockQueryable.DataSource.First().InstanceDataSource.Where(x => x < 5),
                             firstGreaterThan7 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(firstGreaterThan7Predefined, firstGreaterThan7));
        }

        [TestMethod]
        public void GroupJoinQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var query = (from fuVisited in source
                         join fuVisitor in source
                         on true equals true
                         into visitors
                         select new { Visited = fuVisited.Nickname, Visitors = visitors })
                        .AsMockQueryable();

            var groupJoinedForumUsersPredefined = from fuVisited in ForumUsersMockQueryable.DataSource
                                             join fuVisitor in ForumUsersMockQueryable.DataSource
                                             on true equals true
                                             into visitors
                                             select new { Visited = fuVisited.Nickname, Visitors = visitors };
            var groupJoinedForumUsers = query;

            foreach ((var groupJoinedForumUserPredefinedData, var groupJoinedForumUserData) 
                     in groupJoinedForumUsersPredefined.Zip(groupJoinedForumUsers))
            {
                Assert.IsTrue(groupJoinedForumUserPredefinedData.Visited == groupJoinedForumUserData.Visited);
                Assert.IsTrue(Enumerable.SequenceEqual(groupJoinedForumUserPredefinedData.Visitors, groupJoinedForumUserData.Visitors));
            }
        }

        [TestMethod]
        public void JoinQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var query = (from fuVisited in source
                                   join fuVisitor in source
                                   on true equals true
                                   select new { Visited = fuVisited.Nickname, Visitor = fuVisitor.Nickname })
                                   .AsMockQueryable();

            var joinedForumUsersPredefined = from fuVisited in ForumUsersMockQueryable.DataSource
                                              join fuVisitor in ForumUsersMockQueryable.DataSource
                                              on true equals true
                                              select new { Visited = fuVisited.Nickname, Visitor = fuVisitor.Nickname };
            var joinedForumUsers = query;

            Assert.IsTrue(Enumerable.SequenceEqual(joinedForumUsersPredefined, joinedForumUsers));
        }

        [TestMethod]
        public void JoinWhereQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var query = (from visitingInfo in (
                                       from fuVisited in source
                                       join fuVisitor in source
                                       on true equals true
                                       select new { 
                                           Visited = fuVisited.Nickname, 
                                           Visitor = fuVisitor })
                                   where visitingInfo.Visitor.LikeCount > 99
                                   select visitingInfo).AsMockQueryable();

            var joinedForumUsersWhereLikesCountGreaterThan99Predefined = from visitingInfo in (
                                                from fuVisited in ForumUsersMockQueryable.DataSource
                                                join fuVisitor in ForumUsersMockQueryable.DataSource
                                                on true equals true
                                                select new { Visited = fuVisited.Nickname, Visitor = fuVisitor })
                                             where visitingInfo.Visitor.LikeCount > 99
                                             select visitingInfo;
            var joinedForumUsersWhereLikesCountGreaterThan99 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(joinedForumUsersWhereLikesCountGreaterThan99Predefined, joinedForumUsersWhereLikesCountGreaterThan99));
        }

        [TestMethod]
        public void JoinAnyQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            bool fromJoinedForumUsersAnyHas100Likes = (
                                       from fuVisited in source
                                       join fuVisitor in source
                                       on true equals true
                                       select new
                                       {
                                           Visited = fuVisited.Nickname,
                                           Visitor = fuVisitor
                                       }
                                    ).AsMockQueryable()
                                    .Any(visitingInfo => visitingInfo.Visitor.LikeCount > 99);

            bool fromJoinedForumUsersAnyHas100LikesPredefined = (
                                                from fuVisited in ForumUsersMockQueryable.DataSource
                                                join fuVisitor in ForumUsersMockQueryable.DataSource
                                                on true equals true
                                                select new { Visited = fuVisited.Nickname, Visitor = fuVisitor }
                                             )
                                             .Any(visitingInfo => visitingInfo.Visitor.LikeCount > 99);

            Assert.IsTrue(fromJoinedForumUsersAnyHas100LikesPredefined == fromJoinedForumUsersAnyHas100Likes);
        }


        [TestMethod]
        public void TakeQuery()
        {
            GenericMockQueryable<int> source = new MockQueryable();
            GenericMockQueryable<int> query = source.Take(QueryContent.Number10).AsMockQueryable();

            IEnumerable<int> taken10Predefined = MockQueryable.DataSource.Take(10),
                             taken10 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(taken10Predefined, taken10));
        }

        /// <summary>
        /// Тест предназначен для проверки корректности того, что конвейер запросов
        /// останавливается при остановке любого из компонентов.
        /// В нормальной ситуации при получении первого чётного числа (квадрат двух)
        /// конвейер остановится, в ненормальной ситуации запрос SelectMany не сообщит запросу Take,
        /// что конвейер должен остановиться.
        /// </summary>
        [TestMethod]
        public void TakeWhileTakeSelectManyTakeWhileQuery()
        {
            GenericMockQueryable<int> source = new MockQueryable();
            IEnumerable<int> query = Queryable.SelectMany(source.TakeWhile(QueryContent.GreaterThan0).AsMockQueryable().Take(QueryContent.Number3).AsMockQueryable()
                , (x) => Pow2To4(x), (x, y) => y).AsMockQueryable()
                .TakeWhile(QueryContent.IsOdd);

            IEnumerable<int> taken10Predefined = MockQueryable.DataSource
                .TakeWhile(x => x > 0).Take(3).SelectMany(Pow2To4, (x, y) => y).TakeWhile(x => (x & 0b1) == 1),
                             taken10 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(taken10Predefined, taken10));
        }

        private IEnumerable<int> Pow2To4(int x)
        {
            for (int i = 2; i <= 4; i++)
            {
                yield return (int)Math.Pow(x, i);
            }

            yield break;
        }


        [TestMethod]
        public void TakeWhileJoinQuery()
        {
            GenericMockQueryable<int> source = new MockQueryable().TakeWhile(QueryContent.LesserThan10).AsMockQueryable();
            GenericMockQueryable<int> query = source
                .TakeWhile<int>(QueryContent.LesserThan10).AsMockQueryable()
                .Join<int, int, int>(
                    source.TakeWhile<int>(QueryContent.LesserThan10), 
                    QueryContent.IsEven, 
                    QueryContent.Mod3Is0, 
                    QueryContent.Mod6).AsMockQueryable();

            IEnumerable<int> takenWhileLesserThan10JoinIsOddOnMod3Is0ToMod6Predefined = MockQueryable.DataSource
                .TakeWhile(data => data < 10)
                .Join(
                    MockQueryable.DataSource.TakeWhile(data => data < 10), 
                    outerData => (outerData & 1) == 0, 
                    innerData => innerData % 3 == 0, 
                    (outerData, innerData) => outerData == innerData ? outerData : 0),
                             takenWhileLesserThan10JoinIsOddOnMod3Is0ToMod6 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(takenWhileLesserThan10JoinIsOddOnMod3Is0ToMod6Predefined, takenWhileLesserThan10JoinIsOddOnMod3Is0ToMod6));
        }

        [TestMethod]
        public void SelectQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Select<int, bool>(QueryContent.IsOdd).AsMockQueryable();

            IEnumerable<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Select(data => (data & 1) == 1),
                      selectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, selectIsOdd));
        }

        [TestMethod]
        public void SelectQuery2()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<float> query = (from data in source select 1f / data).AsMockQueryable();

            IEnumerable<float> selectIsOddPredefined =
                MockQueryable.DataSource.Select(data => 1f / data),
                      selectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, selectIsOdd));
        }

        [TestMethod]
        public void SelectSkipQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Select<int, bool>(QueryContent.IsOdd).AsMockQueryable()
                .Skip(QueryContent.Number10).AsMockQueryable()
                .Skip(QueryContent.Number1000).AsMockQueryable();

            IEnumerable<bool> selectIsOddAndSkip1000Predefined =
                MockQueryable.DataSource.Select(data => (data & 1) == 1).Skip(10).Skip(1000),
                      selectIsOddAndSkip1000 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddAndSkip1000Predefined, selectIsOddAndSkip1000));
        }

        [TestMethod]
        public void SelectTakeQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Select<int, bool>(QueryContent.IsOdd).AsMockQueryable()
                .Take(QueryContent.Number1000).AsMockQueryable();

            IEnumerable<bool> selectIsOddAndTake1000Predefined =
                MockQueryable.DataSource.Select(data => (data & 1) == 1).Take(1000),
                              selectIsOddAndTake1000 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddAndTake1000Predefined, selectIsOddAndTake1000));
        }

        [TestMethod]
        public void TakeSelectTakeQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Take(QueryContent.Number10).AsMockQueryable()
                .Select<int, bool>(QueryContent.IsOdd).AsMockQueryable()
                .Take(QueryContent.Number1000).AsMockQueryable();

            IEnumerable<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Take(10).Select(data => (data & 1) == 1).Take(1000),
                      selectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, selectIsOdd));
        }

        [TestMethod]
        public void SelectManyQuery()
        {
            GenericMockQueryable<ForumUser> source = new ForumUsersMockQueryable();
            var selectMany = (from fu1 in source
                              from fu2 in source
                              select new { Visited = fu1, Visitor = fu2 }).AsMockQueryable();

            var selectManyPredefined = (from fu1 in ForumUsersMockQueryable.DataSource
                                        from fu2 in ForumUsersMockQueryable.DataSource
                                        select new { Visited = fu1, Visitor = fu2 });

            Assert.IsTrue(Enumerable.SequenceEqual(selectManyPredefined, selectMany));
        }


        [TestMethod]
        public void SkipQuery()
        {
            GenericMockQueryable<int> source = new MockQueryable();
            GenericMockQueryable<int> query = source.Skip(QueryContent.Number10).AsMockQueryable();

            IEnumerable<int> skipped10Predefined = MockQueryable.DataSource.Skip(10),
                             skipped10 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(skipped10Predefined, skipped10));
        }

        [TestMethod]
        public void SkipWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .SkipWhile(QueryContent.LesserThan5).AsMockQueryable();

            IEnumerable<int> skippedWhileLesserThan5Predefined =
                MockQueryable.DataSource.SkipWhile(data => data < 5),
                             skippedWhileLesserThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(skippedWhileLesserThan5Predefined, skippedWhileLesserThan5));
        }

        [TestMethod]
        public void SkipWhileQuery2()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .SkipWhile(QueryContent.GreaterThan5).AsMockQueryable();

            IEnumerable<int> skippedWhileGreaterThan5Predefined =
                MockQueryable.DataSource.SkipWhile(data => data > 5),
                             skippedWhileGreaterThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(skippedWhileGreaterThan5Predefined, skippedWhileGreaterThan5));
        }

        [TestMethod]
        public void TakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .TakeWhile(QueryContent.LesserThan5).AsMockQueryable();

            IEnumerable<int> takenWhileLesserThan5Predefined =
                MockQueryable.DataSource.TakeWhile(data => data < 5),
                             takenWhileLesserThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(takenWhileLesserThan5Predefined, takenWhileLesserThan5));
        }

        [TestMethod]
        public void WhereQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source.Where(QueryContent.GreaterThan5).AsMockQueryable();

            IEnumerable<int> whereGreaterThan5Predefined = MockQueryable.DataSource.Where(data => data > 5),
                             whereGreaterThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(whereGreaterThan5Predefined, whereGreaterThan5));
        }

        [TestMethod]
        public void WhereSelectQuery()
        {
            MockQueryable source = new MockQueryable();
            IQueryable<bool> query = source
                .Where(QueryContent.LesserThan10).AsMockQueryable()
                .Select<int, bool>(QueryContent.IsOdd);

            IEnumerable<bool> whereLesserThan10SelectIsOddPredefined =
                MockQueryable.DataSource.Where(data => data < 10).Select(data => (data & 1) == 1),
                              whereLesserThan10SelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(whereLesserThan10SelectIsOddPredefined, whereLesserThan10SelectIsOdd));
        }

        [TestMethod]
        public void WhereSelectQuery2()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query =
                (from data in source
                 where data < 10
                 let data2 = data ^ 3
                 select data % 1 == 1 || data2 % 3 == 0).AsMockQueryable();

            IEnumerable<bool> whereLesserThan10SelectIsOddPredefined =
                (from data in MockQueryable.DataSource
                 where data < 10
                 let data2 = data ^ 3
                 select data % 1 == 1 || data2 % 3 == 0),
                              whereLesserThan10SelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(whereLesserThan10SelectIsOddPredefined, whereLesserThan10SelectIsOdd));
        }

        [TestMethod]
        public void WhereWhereQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .Where(QueryContent.GreaterThan5).AsMockQueryable()
                .Where(QueryContent.LesserThan10).AsMockQueryable();

            IEnumerable<int> whereGreaterThan5AndLesserThan10Predefined = 
                MockQueryable.DataSource.Where(data1 => data1 > 5).Where(data2 => data2 < 10),
                             whereGreaterThan5AndLesserThan10 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(whereGreaterThan5AndLesserThan10Predefined, whereGreaterThan5AndLesserThan10));
        }

        [TestMethod]
        public void WhereTakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .Where(QueryContent.GreaterThan5).AsMockQueryable()
                .TakeWhile(QueryContent.LesserThan10).AsMockQueryable();

            IEnumerable<int> whereGreaterThan5AndTakenWhileLesserThan10Predefined =
                MockQueryable.DataSource.Where(data => data > 5).Where(data => data < 10),
                             whereGreaterThan5AndTakenWhileLesserThan10 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(
                whereGreaterThan5AndTakenWhileLesserThan10Predefined, whereGreaterThan5AndTakenWhileLesserThan10));
        }

        [TestMethod]
        public void TakeWhileTakeWhileTakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .TakeWhile(QueryContent.LesserThan10).AsMockQueryable()
                .TakeWhile(QueryContent.GreaterThan0).AsMockQueryable()
                .TakeWhile(QueryContent.LesserThan5).AsMockQueryable();

            IEnumerable<int> takenWhileLesserThan10AndGreaterThan0AndLesserThan5Predefined =
                MockQueryable.DataSource.TakeWhile(data => data < 5).Where(data => data < 10),
                             takenWhileLesserThan10AndGreaterThan0AndLesserThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(
                takenWhileLesserThan10AndGreaterThan0AndLesserThan5Predefined, takenWhileLesserThan10AndGreaterThan0AndLesserThan5));
        }

        [TestMethod]
        public void WhereAllQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Where(QueryContent.GreaterThan5).AsMockQueryable()
                               .All(QueryContent.GreaterThan0);

            bool whereGreaterThan5AllGreaterThan0Predefined = 
                MockQueryable.DataSource.Where(data => data > 5).All(data => data > 0),
                 whereGreaterThan5AllGreaterThan0 = query;

            Assert.IsTrue(whereGreaterThan5AllGreaterThan0Predefined == whereGreaterThan5AllGreaterThan0);
        }

        [TestMethod]
        public void WhereAnyQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Where(QueryContent.GreaterThan5).AsMockQueryable()
                               .Any(QueryContent.LesserThan10);

            bool whereGreaterThan5AllLesserThan10Predefined = 
                MockQueryable.DataSource.Where(data => data > 5).Any(data => data < 10),
                 whereGreaterThan5AllLesserThan10 = query;

            Assert.IsTrue(whereGreaterThan5AllLesserThan10Predefined == whereGreaterThan5AllLesserThan10);
        }

        [TestMethod]
        public void ToArrayQuery()
        {
            MockQueryable source = new MockQueryable();
            int[] query = source.ToArray();

            IEnumerable<int> arrayedPredefined = MockQueryable.DataSource.ToArray(),
                             arrayed = query;

            Assert.IsTrue(Enumerable.SequenceEqual(arrayedPredefined, arrayed));
        }

        [TestMethod]
        public void ToDictionaryQuery()
        {
            MockQueryable source = new MockQueryable();
            Dictionary<int, int> query = 
                source.ToDictionary<int, int, int>(QueryContent.All, QueryContent.Pow2);

            Dictionary<int, int> dictionariedPredefined = 
                MockQueryable.DataSource.ToDictionary(x => x, (x) => x ^ 2),
                                 dictionaried = query;

            Assert.IsTrue(dictionariedPredefined.Keys.Count == dictionaried.Keys.Count);

            foreach (var key in dictionariedPredefined.Keys)
            {
                Assert.IsTrue(dictionaried.ContainsKey(key));
                Assert.AreEqual(dictionariedPredefined[key], dictionaried[key]);
            }
        }

        [TestMethod]
        public void ToListQuery()
        {
            MockQueryable source = new MockQueryable();
            List<int> query = source.ToList();

            IEnumerable<int> listedPredefined = MockQueryable.DataSource.ToList(),
                             listed = query;

            Assert.IsTrue(Enumerable.SequenceEqual(listedPredefined, listed));
        }

        [TestMethod]
        public void TakeToArrayQuery()
        {
            MockQueryable source = new MockQueryable();
            int[] query = source.Take(QueryContent.Number10).AsMockQueryable()
                                .ToArray();

            IEnumerable<int> takenArrayedPredefined = MockQueryable.DataSource.Take(10).ToArray(),
                             takenArrayed = query;

            Assert.IsTrue(Enumerable.SequenceEqual(takenArrayedPredefined, takenArrayed));
        }
    }
}
