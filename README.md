
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
![ezgif-4550a4d8448480ea](https://github.com/user-attachments/assets/9abe03d6-76a8-435f-a6f2-4e6e91ab528c)

![ezgif-4a730dc48e4fb0e3](https://github.com/user-attachments/assets/59fae58f-bac5-41d4-b9bd-652ef57317cf)

![ezgif-47438e588eb1344f](https://github.com/user-attachments/assets/d79d960e-ca70-4bfe-9e87-d6dcd62174fe)

![docoment](https://github.com/user-attachments/assets/a541823f-e060-4b34-ab62-640498433fec)

![ezgif-4d3bb5f19d88dca5](https://github.com/user-attachments/assets/14a81360-6b35-4b3d-868c-17ac8109b1d8)

![ezgif-435a859cf1354edb](https://github.com/user-attachments/assets/58f4b0b6-515d-40bd-84c2-0508de264037)



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


