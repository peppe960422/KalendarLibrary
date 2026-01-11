using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    public interface ITermin
    {
        DateTime Start { get; set; }
        DateTime End { get; set; }

        string[] GetInfoData();


    }
    public interface IHaveAWorkspace
    {
        Panel WorkSpace { get; }

        Control CurrentControl { get; set; }
    }
    public interface ITerminProperty
    {

        string GetDisplayName();

    }

    public interface IDataProvider<T> where T : ITermin
    {
        Task<List<ITermin>> GetItems(DateTime start, DateTime end);
        Task SaveChanges(IEnumerable<ITermin> newItems, IEnumerable<ITermin> updatedItems, IEnumerable<ITermin> deletedItems);
    }


    public interface ITerminGenerator
    {

        public List<ITermin> GenerateTermins();

        public void SetCreateButtonEvent(EventHandler Handler);




    }

}
