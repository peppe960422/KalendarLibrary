
# Generic Calendar – Workspace & Termin Architecture
🇬🇧 English Documentation
## Overview
This project defines a generic calendar architecture based on interfaces to ensure flexibility, extensibility, and clean separation of concerns.

The system is built around:

A workspace-aware form

A data provider abstraction

appointment (Termin) entities

appointment properties
## 🎥 Demo Video

[![Calendar Demo](https://img.youtube.com/vi/E1fw_KYM72I/0.jpg)](https://www.youtube.com/watch?v=E1fw_KYM72I)


## Interfaces
**ITermin**

Represents a calendar appointment.
````c#
public interface ITermin
{
    DateTime Start { get; set; }
    DateTime End { get; set; }

    string[] GetInfoData();
}
`````

All appointment objects must implement this interface.

**ITerminProperty**

Represents a displayable property related to an appointment.
```c#
public interface ITerminProperty
{
    string GetDisplayName();
}
```

All properties related to appointments (e.g. subjects, categories, tests) must implement this interface.

**IDataProvider<T>**

Defines the data access layer for calendar appointments.
```c#
public interface IDataProvider<T> where T : ITermin
{
    Task<List<ITermin>> GetItems(DateTime start, DateTime end);
    Task SaveChanges(
        IEnumerable<ITermin> newItems,
        IEnumerable<ITermin> updatedItems,
        IEnumerable<ITermin> deletedItems
    );
}
```

The repository or data source must implement this interface.

**IHaveAWorkspace**

Defines a form that hosts dynamic user controls inside a workspace panel.
```c#
public interface IHaveAWorkspace
{
    Panel WorkSpace { get; }
    IUserControlWithPfad CurrentControl { get; set; }
}
```
**Workspace Form Implementation**

Example implementation using Windows Forms:
```c#
public partial class Form2 : Form, IHaveAWorkspace
{
    Panel _workSpace = new Panel();

    public Form2()
    {
        _workSpace.Dock = DockStyle.Fill;

        InitializeComponent();
        this.DoubleBuffered = true;
        this.WindowState = FormWindowState.Maximized;

        Kalendar kalendar = new Kalendar(new DateTime(2025, 1, 6));
        FakeDataProvider dataProvider = new FakeDataProvider();

        List<Fach> fachList = new()
        {
            new Fach { Name = "Mathe" },
            new Fach { Name = "Deutsch" },
            new Fach { Name = "Englisch" }
        };

        List<Test2> tests = new()
        {
            new Test2 { Name = "TestA" },
            new Test2 { Name = "TestB" }
        };

        List<SingleTest> singleTests = new()
        {
            new SingleTest()
        };

        List<List<ITerminProperty>> properties = new()
        {
            fachList.Cast<ITerminProperty>().ToList(),
            singleTests.Cast<ITerminProperty>().ToList(),
            tests.Cast<ITerminProperty>().ToList()
        };

        Termin termin = new Termin();

        this.Controls.Add(_workSpace);

        GenericKalendarView calendarView =
            new GenericKalendarView(
                kalendar,
                dataProvider,
                properties,
                days: 5,
                hours: 12,
                dayBegin: 8,
                termin
            );

        calendarView.Location = new Point(10, 10);
        _workSpace.Controls.Add(calendarView);
    }

    public Panel WorkSpace => _workSpace;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Control CurrentControl { get; set; }
}
```
## GenericKalendarView Constructor
```c#
GenericKalendarView(
    Kalendar kalendar,
    IDataProvider<ITermin> dataProvider,
    List<List<ITerminProperty>> terminProperties,
    int days,
    int hours,
    int dayBegin,
    ITermin templateTermin
)
```
| Parameter | Description |
|-----|----------|
| kalendar| Base calendar model |
| dataProvider | Data source for appointments |
| terminProperties| appointment properties |
| days	 | 1Number of visible days of the week  |
| hours| Hours per day|
| dayBegin | 	First visible hour |
| templateTermin | 	Appointment template |


