using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TimeFillets.Model;

namespace TimeFillets.Logic
{
  public static class Searcher
  {
    public static List<CalendarItem> Search(string searchBy, string searchValue, IEnumerable<CalendarItem> searchCollection)
    {
      List<CalendarItem> ret = new List<CalendarItem>();
      switch (searchBy)
      {
        case "Title":
          ret = searchCollection.Where(itm => itm.Title.ToLower().Contains(searchValue.ToLower())).ToList();
          break;
        case "Customer":
          ret = searchCollection.Where(itm => itm.CustomerItem.Name.ToLower().Contains(searchValue.ToLower())).ToList();
          break;
        case "Project":
          ret = searchCollection.Where(itm => itm.ProjectItem.Name.ToLower().Contains(searchValue.ToLower())).ToList();
          break;
        case "Task":
          ret = searchCollection.Where(itm => itm.TaskItem.Name.ToLower().Contains(searchValue.ToLower())).ToList();
          break;
        case "StartedBefore":
          ret = searchCollection.Where(itm => itm.StartDate < DateTime.Parse(searchValue)).ToList();
          break;
        case "StartedAfter":
          ret = searchCollection.Where(itm => itm.StartDate > DateTime.Parse(searchValue)).ToList();
          break;
        case "EndedBefore":
          ret = searchCollection.Where(itm => itm.EndDate < DateTime.Parse(searchValue)).ToList();
          break;
        case "EndedAfter":
          ret = searchCollection.Where(itm => itm.EndDate > DateTime.Parse(searchValue)).ToList();
          break;
      }
      return ret;
    }
  }
}
