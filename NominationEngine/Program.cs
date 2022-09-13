using System.Linq.Expressions;

internal class Program
{
    static List<Setting> countriesRank = new List<Setting>
            {
                new Setting{Id=1,Rank=1},
                new Setting{Id=2,Rank=3},
                new Setting{Id=3,Rank=2},
            };
    static List<Setting> specializationsRank = new List<Setting>
            {
                new Setting{Id=1,Rank=1},
                new Setting{Id=2,Rank=3},
                new Setting{Id=3,Rank=2},
            };
    static List<Setting> studyFieldsRank = new List<Setting>
            {
                new Setting{Id=2,Rank=3},
                new Setting{Id=1,Rank=1},
                new Setting{Id=3,Rank=2},
            };

    static List<NominationCriteria> piplineByPath = new List<NominationCriteria>
        {
           new NominationCriteria()
           {
               Priorty=2,
               FieldToOrderBy=(s)=>s.CountryRank,
               AllowedValues=new List<int>{ 1,2},
               LJoinKey=st=>st.CountryId,
               RJoinKey=b=>b.Id,
               RetunFunc=(st,b)=>{st.CountryRank=b.Rank; return st; },
               FileterByAllowedValues=(st,lst)=>lst.Contains(st.CountryId),
               Data=countriesRank,
               TableName=Table.Country
           },
           new NominationCriteria()
           {
               Priorty=1,
               FieldToOrderBy=(s)=>s.SpecializationRank,
               LJoinKey=st=>st.SpecializationId,
               RJoinKey=b=>b.Id,
               RetunFunc=(st,b)=>{st.SpecializationRank=b.Rank; return st; },
               FileterByAllowedValues=(st,lst)=>lst.Contains(st.SpecializationId),
               Data=specializationsRank,
               TableName=Table.Specialization
           },
           new NominationCriteria()
           {
               Priorty=3,
               FieldToOrderBy=(s)=>s.StudyFieldRank,
               LJoinKey=st=>st.StudyFieldId,
               RJoinKey=b=>b.Id,
               RetunFunc=(st,b)=>{st.StudyFieldRank=b.Rank; return st; },
               FileterByAllowedValues=(st,lst)=>lst.Contains(st.StudyFieldId),
               Data=studyFieldsRank,
               TableName=Table.StudyField
           }
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
        piplineByPath = piplineByPath.OrderBy(c => c.Priorty).ToList();
        foreach (var criteria in piplineByPath)
        {
            query = QueryBuilder.Build(query, criteria);
        }

        var top = piplineByPath.First();

        IOrderedQueryable<Student> q = query.OrderBy(top.FieldToOrderBy);
        foreach (var criteria in piplineByPath)
        {
            if (criteria.TableName == top.TableName) continue;
            q = q.ThenBy(criteria.FieldToOrderBy);
        }
        return q;
    }
}
public static class QueryBuilder
{
    public static IQueryable<Student> Build(IQueryable<Student> query, NominationCriteria criteria)
    {
        if (criteria?.AllowedValues?.Any() ?? false)
            query = query.Where(x => criteria.FileterByAllowedValues(x, criteria.AllowedValues));
        return query.Join(criteria.Data, criteria.LJoinKey, criteria.RJoinKey, (st, b) => criteria.RetunFunc(st, b));
    }
}


public class NominationCriteria
{
    public Table TableName { get; set; }
    public int Priorty { get; set; }
    public List<int> AllowedValues { get; set; } = new();
    public int AvailableSeats { get; set; }
    public Expression<Func<Student, int>> FieldToOrderBy { get; set; }
    public Expression<Func<Student, int>> LJoinKey { get; set; }
    public Expression<Func<Setting, int>> RJoinKey { get; set; }
    public Func<Student, Setting, Student> RetunFunc { get; set; }
    public Func<Student, List<int>, bool> FileterByAllowedValues { get; set; }
    public List<Setting> Data { get; set; }
}
public enum Table
{
    Country = 1,
    Specialization = 2,
    StudyField = 3
}
public class Student
{
    public string NationalId { get; set; }
    public int CountryId { get; set; }
    public int SpecializationId { get; set; }
    public int StudyFieldId { get; set; }

    public int PathId { get; set; }
    public int CountryRank { get; set; }
    public int SpecializationRank { get; set; }
    public int StudyFieldRank { get; set; }



}
public class Setting
{
    public int Id { get; set; }
    public int Rank { get; set; }
}
