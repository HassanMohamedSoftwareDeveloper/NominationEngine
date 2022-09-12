//using System.Dynamic;

//namespace tt;
//internal class Program1
//{
//    List<NominationCriteria<int>> piplineByPath = new List<NominationCriteria<int>>
//        {
//           new NominationCriteria<int>() { Priorty=1, FieldToCheck=FieldsToCheck.CountryId,TableName=Table.Country,AllowedValues=new List<int>{ 1,2} },
//           new NominationCriteria<int>() { Priorty=2, TableName=Table.Specialization },
//           new NominationCriteria<int>() { Priorty=3, TableName=Table.StudyField}
//        };

//    List<Country> countriesRank = new List<Country>
//        {
//            new Country{Id=1,Rank=1},
//            new Country{Id=2,Rank=3},
//            new Country{Id=3,Rank=2},
//        };
//    List<Specialization> specializationsRank = new List<Specialization>
//        {
//            new Specialization{Id=1,Rank=1},
//            new Specialization{Id=2,Rank=3},
//            new Specialization{Id=3,Rank=2},
//        };
//    List<StudyField> studyFieldsRank = new List<StudyField>
//        {
//            new StudyField{Id=2,Rank=3},
//            new StudyField{Id=1,Rank=1},
//            new StudyField{Id=3,Rank=2},
//        };

//    List<Student> studentsByPath = new List<Student>()
//        {
//                new Student() { NationalId="1", CountryId=1, SpecializationId=2, StudyFieldId=2},
//                new Student() { NationalId="2", CountryId=2, SpecializationId=3, StudyFieldId=1},
//                new Student() { NationalId="3", CountryId=3, SpecializationId=1, StudyFieldId=3},
//        };
//    static void Main(string[] args)
//    {
//        dynamic c1 = new ExpandoObject();
//        c1.Student = new Student() { NationalId = "1111" };

//        Console.WriteLine(c1.Student.NationalId);
//        Console.ReadKey();
//    }

//    public IQueryable<dynamic> BuildExpression()
//    {
//        IQueryable<dynamic> query =
//            studentsByPath.
//            Select(x =>
//            {
//                dynamic ex = new ExpandoObject();
//                ex.Student = x;
//                return ex;
//            })
//            .AsQueryable();



//        foreach (var criteria in piplineByPath.OrderBy(c => c.Priorty))
//        {
//            if (criteria.AllowedValues.Any() == false) continue;
//            if (criteria.TableName == Table.Country)
//            {
//                //query = query.Where(x => criteria.AllowedValues.Contains(x.Student.CountryId ));

//                query = query.Join(countriesRank,
//                    stKey => stKey.Student.CountryId,
//                    countryKey => countryKey.Id,
//                    (s, country) => { dynamic res = (ExpandoObject)s; res.CountryRank = country.Rank; return res; }
//                    );

//            }
//            else if (criteria.TableName == Table.Specialization)
//            {

//            }
//        }
//        return query;
//    }
//    private static Func<dynamic, TInner, dynamic> JoinFunc<TInner, TResult>()
//    {
//        return (obj1, obj2) =>
//        {
//            dynamic ex = new ExpandoObject();

//            return ex;
//        };

//    }
//    //private List<Student> RankByCountry(List<Student> students)
//    //{
//    //    var countries = new List<Country> { new Country() { CountryId = 1, CountryRank = 2 }, new Country() { CountryId = 2, CountryRank = 3 }, new Country() { CountryId = 3, CountryRank = 1 } };
//    //    var availableCountries = new List<int> { 1, 2 };
//    //    return (from s in students
//    //            join c in countries on s.Country.CountryId equals c.CountryId
//    //            select new Student
//    //            {
//    //                NationalId = s.NationalId,
//    //                Country = s.Country
//    //            }).Where(c => availableCountries.Contains(c.Country.CountryId)).OrderBy(c => c.Country.CountryRank).ToList();

//    //}

//    //private List<Student> RankBySpecialization(List<Student> students)
//    //{
//    //    var specializations = new List<Specialization> { new Specialization() { SpecializationId = 1, SpecializationRank = 3 },
//    //        new Specialization() { SpecializationId = 2, SpecializationRank = 1}, new Specialization() { SpecializationId = 3, SpecializationRank = 2 } };
//    //    var availableSpecializations = new List<int> { 1, 3 };
//    //    return (from s in students
//    //            join c in specializations on s.Specialization.SpecializationId equals c.SpecializationId
//    //            select new Student
//    //            {
//    //                NationalId = s.NationalId,
//    //                Specialization = s.Specialization
//    //            }).Where(c => availableSpecializations.Contains(c.Specialization.SpecializationId)).OrderBy(c => c.Specialization.SpecializationId).ToList();

//    //}

//    //private List<Student> RankByStudyField(List<Student> students)
//    //{
//    //    var StudyFields = new List<StudyField> { new StudyField() { StudyFieldId = 1, StudyFieldRank = 3 },
//    //        new StudyField() { StudyFieldId = 2, StudyFieldRank = 1}, new StudyField() { StudyFieldId = 3, StudyFieldRank = 2 } };
//    //    var availableStudyFields = new List<int> { 1 };
//    //    return (from s in students
//    //            join c in StudyFields on s.StudyField.StudyFieldId equals c.StudyFieldId
//    //            select new Student
//    //            {
//    //                NationalId = s.NationalId,
//    //                Specialization = s.Specialization
//    //            }).Where(c => availableStudyFields.Contains(c.StudyField.StudyFieldId)).OrderBy(c => c.StudyField.StudyFieldId).ToList();

//    //}
//}

//public enum FieldsToCheck
//{
//    CountryId,
//}
//public enum Table
//{
//    Country = 1,
//    Specialization = 2,
//    StudyField = 3
//}
//public class NominationCriteria<T>
//{
//    public Table TableName { get; set; }
//    public int Priorty { get; set; }
//    public List<T> AllowedValues { get; set; } = new();
//    public FieldsToCheck FieldToCheck { get; set; }
//}

//public class Student
//{
//    public string NationalId { get; set; }
//    public int CountryId { get; set; }
//    public int SpecializationId { get; set; }
//    public int StudyFieldId { get; set; }

//}
//public abstract class BaseSetting
//{
//    public int Id { get; set; }
//    public int Rank { get; set; }
//}
//public sealed class Country : BaseSetting
//{
//}

//public sealed class Specialization : BaseSetting
//{
//}

//public sealed class StudyField : BaseSetting
//{
//}


