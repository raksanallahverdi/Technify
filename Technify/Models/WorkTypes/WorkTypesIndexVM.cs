using Technify.Entities;

namespace Technify.Models.WorkTypes
{
    public class WorkTypesIndexVM
    {
        public List<WorkType> WorkTypes { get; set; }
        public List<Entities.Work> Works { get; set; }
    }
}
