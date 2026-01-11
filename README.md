<?xml version="1.0" encoding="UTF-8"?>
<documentation>
  <metadata>
    <title>Appointment Management System / Terminverwaltungssystem</title>
    <version>1.0.0</version>
    <last-updated>2024-01-15</last-updated>
    <languages>
      <language>English</language>
      <language>German</language>
    </languages>
  </metadata>
  
  <!-- ENGLISH VERSION -->
  <section lang="en" title="Appointment Management System - Technical Documentation">
    
    <h1>Overview</h1>
    <p>This system implements an architecture for appointment management with workspace support, data providers, and customizable properties.</p>
    
    <h1>Architecture Requirements</h1>
    
    <h2>1. Main Form (IHaveAWorkspace)</h2>
    <p>The main form must implement the <code>IHaveAWorkspace</code> interface:</p>
    
    <pre><code class="language-csharp">public partial class Form2 : Form, IHaveAWorkspace
{
    private Panel _workSpace = new Panel();
    
    public Form2()
    {
        _workSpace.Dock = DockStyle.Fill;
        
        InitializeComponent();
        this.DoubleBuffered = true;
        this.WindowState = FormWindowState.Maximized;
        
        // Create calendar
        Kalendar k = new Kalendar(new DateTime(2025, 1, 6));
        
        // Create data provider
        FakeDataProvider f = new FakeDataProvider();
        
        // Create subject properties
        List&lt;Fach&gt; subjects = new List&lt;Fach&gt;();
        subjects.Add(new Fach() { Name = "Math" });
        subjects.Add(new Fach() { Name = "German" });
        subjects.Add(new Fach() { Name = "English" });
        
        // Create test properties
        List&lt;Test2&gt; tests = new List&lt;Test2&gt;();
        tests.Add(new Test2() { Name = "TestA" });
        tests.Add(new Test2() { Name = "TestB" });
        
        // Create single test properties
        List&lt;SingleTest&gt; singleTests = new List&lt;SingleTest&gt;();
        singleTests.Add(new SingleTest());
        
        // Combine all properties
        List&lt;List&lt;ITerminProperty&gt;&gt; propertyLists = new List&lt;List&lt;ITerminProperty&gt;&gt;();
        propertyLists.Add(subjects.Cast&lt;ITerminProperty&gt;().ToList());
        propertyLists.Add(singleTests.Cast&lt;ITerminProperty&gt;().ToList());
        propertyLists.Add(tests.Cast&lt;ITerminProperty&gt;().ToList());
        
        // Create appointment template
        Termin appointmentTemplate = new Termin();
        
        // Add workspace to form
        this.Controls.Add(_workSpace);
        
        // Create and configure calendar view
        GenericKalendarView calendarView = new GenericKalendarView(
            kalendar: k,
            dataProvider: f,
            propertyLists: propertyLists,
            days: 5,
            hours: 12,
            dayBegin: 8,
            termin: appointmentTemplate
        );
        
        calendarView.Location = new Point(10, 10);
        this._workSpace.Controls.Add(calendarView);
    }
    
    public Panel WorkSpace { get { return _workSpace; } }
    
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IUserControlWithPfad CurrentControl { get; set; }
}</code></pre>
    
    <h2>2. GenericKalendarView Constructor Parameters</h2>
    <table>
      <thead>
        <tr>
          <th>Parameter</th>
          <th>Type</th>
          <th>Description</th>
          <th>Default Value</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td><code>kalendar</code></td>
          <td><code>Kalendar</code></td>
          <td>Calendar instance with initial date</td>
          <td>Required</td>
        </tr>
        <tr>
          <td><code>dataProvider</code></td>
          <td><code>IDataProvider&lt;ITermin&gt;</code></td>
          <td>Data provider for appointments</td>
          <td>Required</td>
        </tr>
        <tr>
          <td><code>propertyLists</code></td>
          <td><code>List&lt;List&lt;ITerminProperty&gt;&gt;</code></td>
          <td>Lists of appointment properties</td>
          <td>Required</td>
        </tr>
        <tr>
          <td><code>days</code></td>
          <td><code>int</code></td>
          <td>Number of days to display</td>
          <td><code>5</code></td>
        </tr>
        <tr>
          <td><code>hours</code></td>
          <td><code>int</code></td>
          <td>Hours to display per day</td>
          <td><code>12</code></td>
        </tr>
        <tr>
          <td><code>dayBegin</code></td>
          <td><code>int</code></td>
          <td>Starting hour of the day</td>
          <td><code>8</code></td>
        </tr>
        <tr>
          <td><code>termin</code></td>
          <td><code>ITermin</code></td>
          <td>Appointment template</td>
          <td>Required</td>
        </tr>
      </tbody>
    </table>
    
    <h2>3. Required Interfaces Implementation</h2>
    
    <h3>Appointment Objects (ITermin)</h3>
    <pre><code class="language-csharp">public class Termin : ITermin
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    
    public string[] GetInfoData()
    {
        return new string[]
        {
            $"Start: {Start}",
            $"End: {End}"
        };
    }
}</code></pre>
    
    <h3>Appointment Properties (ITerminProperty)</h3>
    <pre><code class="language-csharp">public class Fach : ITerminProperty
{
    public string Name { get; set; }
    
    public string GetDisplayName()
    {
        return Name;
    }
}

public class Test2 : ITerminProperty
{
    public string Name { get; set; }
    
    public string GetDisplayName()
    {
        return Name;
    }
}

public class SingleTest : ITerminProperty
{
    public string GetDisplayName()
    {
        return "Single Test";
    }
}</code></pre>
    
    <h3>Repository/Data Provider (IDataProvider&lt;T&gt;)</h3>
    <pre><code class="language-csharp">public class FakeDataProvider : IDataProvider&lt;ITermin&gt;
{
    public Task&lt;List&lt;ITermin&gt;&gt; GetItems(DateTime start, DateTime end)
    {
        // Implementation for retrieving appointments
        return Task.FromResult(new List&lt;ITermin&gt;());
    }
    
    public Task SaveChanges(
        IEnumerable&lt;ITermin&gt; newItems,
        IEnumerable&lt;ITermin&gt; updatedItems,
        IEnumerable&lt;ITermin&gt; deletedItems)
    {
        // Implementation for saving changes
        return Task.CompletedTask;
    }
}</code></pre>
    
    <h2>Usage Example</h2>
    <pre><code class="language-csharp">// Initialize the main form
var mainForm = new Form2();

// The workspace is automatically configured with:
// - 5-day calendar view
// - 12-hour display (8 AM to 8 PM)
// - Sample data provider
// - Subject, test, and single test properties</code></pre>
    
    <h2>Implementation Checklist</h2>
    
    <h3>Mandatory Requirements</h3>
    <ul>
      <li><input type="checkbox" checked /> Main form implements <code>IHaveAWorkspace</code></li>
      <li><input type="checkbox" checked /> All appointment objects implement <code>ITermin</code></li>
      <li><input type="checkbox" checked /> All appointment properties implement <code>ITerminProperty</code></li>
      <li><input type="checkbox" checked /> Repository implements <code>IDataProvider&lt;ITermin&gt;</code></li>
      <li><input type="checkbox" checked /> Proper use of <code>GenericKalendarView</code> constructor</li>
    </ul>
    
    <h3>Recommended Practices</h3>
    <ul>
      <li><input type="checkbox" /> Use dependency injection for data providers</li>
      <li><input type="checkbox" /> Implement logging for operations</li>
      <li><input type="checkbox" /> Add exception handling in async methods</li>
      <li><input type="checkbox" /> Implement Unit of Work pattern for transactions</li>
      <li><input type="checkbox" /> Add input validation</li>
    </ul>
    
    <h2>Notes</h2>
    <ul>
      <li>The system uses a 5-day calendar view by default</li>
      <li>Hours displayed: 8:00 AM to 8:00 PM (configurable)</li>
      <li>Property lists support multiple categories of appointment properties</li>
      <li>All data operations are asynchronous</li>
    </ul>
    
  </section>
  
  <!-- GERMAN VERSION -->
  <section lang="de" title="Terminverwaltungssystem - Technische Dokumentation">
    
    <h1>Überblick</h1>
    <p>Dieses System implementiert eine Architektur zur Terminverwaltung mit Workspace-Unterstützung, Datenprovidern und anpassbaren Eigenschaften.</p>
    
    <h1>Architektur-Anforderungen</h1>
    
    <h2>1. Hauptformular (IHaveAWorkspace)</h2>
    <p>Das Hauptformular muss die <code>IHaveAWorkspace</code>-Schnittstelle implementieren:</p>
    
    <pre><code class="language-csharp">public partial class Form2 : Form, IHaveAWorkspace
{
    private Panel _workSpace = new Panel();
    
    public Form2()
    {
        _workSpace.Dock = DockStyle.Fill;
        
        InitializeComponent();
        this.DoubleBuffered = true;
        this.WindowState = FormWindowState.Maximized;
        
        // Kalender erstellen
        Kalendar k = new Kalendar(new DateTime(2025, 1, 6));
        
        // Datenprovider erstellen
        FakeDataProvider f = new FakeDataProvider();
        
        // Facheigenschaften erstellen
        List&lt;Fach&gt; fächer = new List&lt;Fach&gt;();
        fächer.Add(new Fach() { Name = "Mathe" });
        fächer.Add(new Fach() { Name = "Deutsch" });
        fächer.Add(new Fach() { Name = "Englisch" });
        
        // Testeigenschaften erstellen
        List&lt;Test2&gt; tests = new List&lt;Test2&gt;();
        tests.Add(new Test2() { Name = "TestA" });
        tests.Add(new Test2() { Name = "TestB" });
        
        // Einzeltest-Eigenschaften erstellen
        List&lt;SingleTest&gt; einzelTests = new List&lt;SingleTest&gt;();
        einzelTests.Add(new SingleTest());
        
        // Alle Eigenschaften kombinieren
        List&lt;List&lt;ITerminProperty&gt;&gt; eigenschaftenListen = new List&lt;List&lt;ITerminProperty&gt;&gt;();
        eigenschaftenListen.Add(fächer.Cast&lt;ITerminProperty&gt;().ToList());
        eigenschaftenListen.Add(einzelTests.Cast&lt;ITerminProperty&gt;().ToList());
        eigenschaftenListen.Add(tests.Cast&lt;ITerminProperty&gt;().ToList());
        
        // Terminvorlage erstellen
        Termin terminVorlage = new Termin();
        
        // Workspace zum Formular hinzufügen
        this.Controls.Add(_workSpace);
        
        // Kalenderansicht erstellen und konfigurieren
        GenericKalendarView kalenderAnsicht = new GenericKalendarView(
            kalendar: k,
            dataProvider: f,
            propertyLists: eigenschaftenListen,
            days: 5,
            hours: 12,
            dayBegin: 8,
            termin: terminVorlage
        );
        
        kalenderAnsicht.Location = new Point(10, 10);
        this._workSpace.Controls.Add(kalenderAnsicht);
    }
    
    public Panel WorkSpace { get { return _workSpace; } }
    
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IUserControlWithPfad CurrentControl { get; set; }
}</code></pre>
    
    <h2>2. GenericKalendarView Konstruktor-Parameter</h2>
    <table>
      <thead>
        <tr>
          <th>Parameter</th>
          <th>Typ</th>
          <th>Beschreibung</th>
          <th>Standardwert</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td><code>kalendar</code></td>
          <td><code>Kalendar</code></td>
          <td>Kalenderinstanz mit Anfangsdatum</td>
          <td>Erforderlich</td>
        </tr>
        <tr>
          <td><code>dataProvider</code></td>
          <td><code>IDataProvider&lt;ITermin&gt;</code></td>
          <td>Datenprovider für Termine</td>
          <td>Erforderlich</td>
        </tr>
        <tr>
          <td><code>propertyLists</code></td>
          <td><code>List&lt;List&lt;ITerminProperty&gt;&gt;</code></td>
          <td>Listen von Termineigenschaften</td>
          <td>Erforderlich</td>
        </tr>
        <tr>
          <td><code>days</code></td>
          <td><code>int</code></td>
          <td>Anzahl anzuzeigender Tage</td>
          <td><code>5</code></td>
        </tr>
        <tr>
          <td><code>hours</code></td>
          <td><code>int</code></td>
          <td>Anzuzeigende Stunden pro Tag</td>
          <td><code>12</code></td>
        </tr>
        <tr>
          <td><code>dayBegin</code></td>
          <td><code>int</code></td>
          <td>Anfangsstunde des Tages</td>
          <td><code>8</code></td>
        </tr>
        <tr>
          <td><code>termin</code></td>
          <td><code>ITermin</code></td>
          <td>Terminvorlage</td>
          <td>Erforderlich</td>
        </tr>
      </tbody>
    </table>
    
    <h2>3. Erforderliche Schnittstellen-Implementierungen</h2>
    
    <h3>Terminobjekte (ITermin)</h3>
    <pre><code class="language-csharp">public class Termin : ITermin
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    
    public string[] GetInfoData()
    {
        return new string[]
        {
            $"Start: {Start}",
            $"Ende: {End}"
        };
    }
}</code></pre>
    
    <h3>Termineigenschaften (ITerminProperty)</h3>
    <pre><code class="language-csharp">public class Fach : ITerminProperty
{
    public string Name { get; set; }
    
    public string GetDisplayName()
    {
        return Name;
    }
}

public class Test2 : ITerminProperty
{
    public string Name { get; set; }
    
    public string GetDisplayName()
    {
        return Name;
    }
}

public class SingleTest : ITerminProperty
{
    public string GetDisplayName()
    {
        return "Einzeltest";
    }
}</code></pre>
    
    <h3>Repository/Datenprovider (IDataProvider&lt;T&gt;)</h3>
    <pre><code class="language-csharp">public class FakeDataProvider : IDataProvider&lt;ITermin&gt;
{
    public Task&lt;List&lt;ITermin&gt;&gt; GetItems(DateTime start, DateTime end)
    {
        // Implementierung zum Abrufen von Terminen
        return Task.FromResult(new List&lt;ITermin&gt;());
    }
    
    public Task SaveChanges(
        IEnumerable&lt;ITermin&gt; neueElemente,
        IEnumerable&lt;ITermin&gt; aktualisierteElemente,
        IEnumerable&lt;ITermin&gt; gelöschteElemente)
    {
        // Implementierung zum Speichern von Änderungen
        return Task.CompletedTask;
    }
}</code></pre>
    
    <h2>Verwendungsbeispiel</h2>
    <pre><code class="language-csharp">// Hauptformular initialisieren
var mainForm = new Form2();

// Der Workspace wird automatisch konfiguriert mit:
// - 5-Tage-Kalenderansicht
// - 12-Stunden-Anzeige (8 bis 20 Uhr)
// - Beispiel-Datenprovider
// - Fach-, Test- und Einzeltest-Eigenschaften</code></pre>
    
    <h2>Implementierungs-Checkliste</h2>
    
    <h3>Zwingende Anforderungen</h3>
    <ul>
      <li><input type="checkbox" checked /> Hauptformular implementiert <code>IHaveAWorkspace</code></li>
      <li><input type="checkbox" checked /> Alle Terminobjekte implementieren <code>ITermin</code></li>
      <li><input type="checkbox" checked /> Alle Termineigenschaften implementieren <code>ITerminProperty</code></li>
      <li><input type="checkbox" checked /> Repository implementiert <code>IDataProvider&lt;ITermin&gt;</code></li>
      <li><input type="checkbox" checked /> Korrekte Verwendung des <code>GenericKalendarView</code>-Konstruktors</li>
    </ul>
    
    <h3>Empfohlene Praktiken</h3>
    <ul>
      <li><input type="checkbox" /> Dependency Injection für Datenprovider verwenden</li>
      <li><input type="checkbox" /> Logging für Operationen implementieren</li>
      <li><input type="checkbox" /> Ausnahmebehandlung in asynchronen Methoden</li>
      <li><input type="checkbox" /> Unit of Work Pattern für Transaktionen implementieren</li>
      <li><input type="checkbox" /> Eingabevalidierung hinzufügen</li>
    </ul>
    
    <h2>Hinweise</h2>
    <ul>
      <li>Das System verwendet standardmäßig eine 5-Tage-Kalenderansicht</li>
      <li>Angezeigte Stunden: 8:00 bis 20:00 Uhr (konfigurierbar)</li>
      <li>Eigenschaftenlisten unterstützen mehrere Kategorien von Termineigenschaften</li>
      <li>Alle Datenoperationen sind asynchron</li>
    </ul>
    
  </section>
  
</documentation>
