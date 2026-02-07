using System;
using System.ComponentModel;

public class Ticket : INotifyPropertyChanged
{
    private int? _assignedEmployeeId;
    private Employee? _assignedEmployee;
    private TicketStatus _status;
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public TicketStatus Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public int? AssignedEmployeeId
    {
        get => _assignedEmployeeId;
        set
        {
            if (_assignedEmployeeId != value)
            {
                _assignedEmployeeId = value;
                OnPropertyChanged(nameof(AssignedEmployeeId));
                OnPropertyChanged(nameof(AssignedEmployeeName));
            }
        }
    }

    public Employee? AssignedEmployee
    {
        get => _assignedEmployee;
        set
        {
            if (_assignedEmployee != value)
            {
                _assignedEmployee = value;
                // id > object
                _assignedEmployeeId = _assignedEmployee?.Id;
                OnPropertyChanged(nameof(AssignedEmployee));
                OnPropertyChanged(nameof(AssignedEmployeeId));
                OnPropertyChanged(nameof(AssignedEmployeeName));
            }
        }
    }

    // for UI
    public string AssignedEmployeeName => AssignedEmployee?.FullName ?? (AssignedEmployeeId.HasValue ? AssignedEmployeeId.Value.ToString() : string.Empty);

    public string AccessCode { get; set; } = string.Empty;
    public string? ClientContact { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public enum TicketStatus
    {
        New,
        InProgress,
        Done
    }
}
