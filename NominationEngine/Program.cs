internal class Program
{
    static List<NominationCriteria<int>> piplineByPath = new List<NominationCriteria<int>>
        {
           new NominationCriteria<int>() { Priorty=1, FieldToCheck=FieldsToCheck.CountryId,TableName=Table.Country,AllowedValues=new List<int>{ 1,2} },
           new NominationCriteria<int>() { Priorty=2, TableName=Table.Specialization },
           new NominationCriteria<int>() { Priorty=3, TableName=Table.StudyField}
        };

    static void Main(string[] args)
    {
        List<Student> studentsByPath = new List<Student>()
        {
                new Student() { NationalId="1", CountryId=1, SpecializationId=2, StudyFieldId=2},
                new Student() { NationalId="2", CountryId=2, SpecializationId=3, StudyFieldId=1},
                new Student() { NationalId="3", CountryId=3, SpecializationId=1, StudyFieldId=3},
                new Student() { NationalId="4", CountryId=1, SpecializationId=3, StudyFieldId=3},
        };
        var StudentsRanked = BuildExpression(studentsByPath).ToList();
        Console.ReadKey();
    }

    public static IQueryable<Student> BuildExpression(List<Student> studentsByPath)
    {
        var query = studentsByPath.AsQueryable();

        foreach (var criteria in piplineByPath.OrderBy(c => c.Priorty))
        {
            if (criteria.TableName == Table.Country)
            {
                query = Filter.FilterByCountry(query, criteria);
            }
            else if (criteria.TableName == Table.Specialization)
            {
                query = Filter.FilterBySpecialization(query, criteria);
            }
            else if (criteria.TableName == Table.StudyField)
            {
                query = Filter.FilterByStudyField(query, criteria);
            }
        }
        bool isOrdered = false;
        IOrderedQueryable<Student> q = (IOrderedQueryable<Student>)query;
        foreach (var criteria in piplineByPath.OrderBy(c => c.Priorty))
        {
            if (criteria.TableName == Table.Country)
            {
                if (isOrdered)
                    q = q.ThenBy(x => x.CountryRank);
                else
                {
                    q = query.OrderBy(x => x.CountryRank);
                }
                isOrdered = true;
            }
            else if (criteria.TableName == Table.Specialization)
            {
                if (isOrdered)
                    q = q.ThenBy(x => x.SpecializationRank);
                else
                {
                    q = query.OrderBy(x => x.SpecializationRank);
                }
                isOrdered = true;
            }
            else if (criteria.TableName == Table.StudyField)
            {
                if (isOrdered)
                    q = q.ThenBy(x => x.StudyFieldRank);
                else
                {
                    q = query.OrderBy(x => x.StudyFieldRank);
                }
                isOrdered = true;
            }
        }
        return query;
    }


}
public class Filter
{
    public static IQueryable<Student> FilterByCountry(IQueryable<Student> query, NominationCriteria<int> criteria)
    {
        List<Country> countriesRank = new List<Country>
        {
            new Country{Id=1,Rank=1},
            new Country{Id=2,Rank=3},
            new Country{Id=3,Rank=2},
        };

        if (criteria?.AllowedValues?.Any() ?? false)
            query = query.Where(x => criteria.AllowedValues.Contains(x.CountryId));

        return query.Join(countriesRank,
             stKey => stKey.CountryId,
             countryKey => countryKey.Id,
             (student, country) => GetStudent(student, country, criteria.TableName));

    }
    public static IQueryable<Student> FilterBySpecialization(IQueryable<Student> query, NominationCriteria<int> criteria)
    {
        List<Specialization> specializationsRank = new List<Specialization>
        {
            new Specialization{Id=1,Rank=1},
            new Specialization{Id=2,Rank=3},
            new Specialization{Id=3,Rank=2},
        };

        if (criteria?.AllowedValues?.Any() ?? false)
            query = query.Where(x => criteria.AllowedValues.Contains(x.SpecializationId));

        return query.Join(specializationsRank,
             stKey => stKey.SpecializationId,
             specializationKey => specializationKey.Id,
             (student, specialization) => GetStudent(student, specialization, criteria.TableName));
    }

    public static IQueryable<Student> FilterByStudyField(IQueryable<Student> query, NominationCriteria<int> criteria)
    {
        List<StudyField> studyFieldsRank = new List<StudyField>
        {
            new StudyField{Id=2,Rank=3},
            new StudyField{Id=1,Rank=1},
            new StudyField{Id=3,Rank=2},
        };

        if (criteria?.AllowedValues?.Any() ?? false)
            query = query.Where(x => criteria.AllowedValues.Contains(x.SpecializationId));

        return query.Join(studyFieldsRank,
            stKey => stKey.StudyFieldId,
            studyFieldKey => studyFieldKey.Id,
            (student, specialization) => GetStudent(student, specialization, criteria.TableName));
    }


    private static Student GetStudent(Student student, BaseSetting baseSetting, Table table)
    {
        if (table == Table.Country)
        {
            student.CountryRank = baseSetting.Rank;
        }
        else if (table == Table.Specialization)
        {
            student.SpecializationRank = baseSetting.Rank;
        }
        else if (table == Table.StudyField)
        {
            student.StudyFieldRank = baseSetting.Rank;
        }
        return student;
    }
}
public enum FieldsToCheck
{
    CountryId,
}
public enum Table
{
    Country = 1,
    Specialization = 2,
    StudyField = 3
}
public class NominationCriteria<T>
{
    public Table TableName { get; set; }
    public int Priorty { get; set; }
    public List<T> AllowedValues { get; set; } = new();
    public FieldsToCheck FieldToCheck { get; set; }
    public int AvailableSeats { get; set; }
}

public class Student
{
    public string NationalId { get; set; }
    public int CountryId { get; set; }
    public int SpecializationId { get; set; }
    public int StudyFieldId { get; set; }

    public int CountryRank { get; set; }
    public int SpecializationRank { get; set; }
    public int StudyFieldRank { get; set; }



}
public abstract class BaseSetting
{
    public int Id { get; set; }
    public int Rank { get; set; }
}
public sealed class Country : BaseSetting
{
}

public sealed class Specialization : BaseSetting
{
}

public sealed class StudyField : BaseSetting
{
}



