namespace DesignPatterns.Proxy.Sample;

// -------------------- Mock Directory/Data --------------------
public static class FakeDirectory
{
    private static readonly Dictionary<Guid, Guid> _managerToDept = new();

    public static void MapManagerToDepartment(Guid managerId, Guid deptId)
        => _managerToDept[managerId] = deptId;

    public static Guid GetDepartmentForManager(Guid managerId)
        => _managerToDept.TryGetValue(managerId, out var dept) ? dept : Guid.Empty;
}