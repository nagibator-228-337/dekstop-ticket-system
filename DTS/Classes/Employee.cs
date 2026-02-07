using System;

public class Employee
{
	public int Id { get; set; }
	public string FullName { get; set; }
	public string Login { get; set; }
	public string PasswordHash { get; set; }
	public Role Role { get; set; }

}

public enum Role
{
    Admin,
    Default
}
