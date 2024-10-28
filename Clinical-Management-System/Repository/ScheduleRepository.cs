using Clinical_Management_System.Data;
using Clinical_Management_System.IRepositery;
using Clinical_Management_System.Models.DB_Entities;
using Clinical_Management_System.Repository.IRepositery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical_Management_System.Repository
{
    public class ScheduleRepository : Repositery<Schedule>, IScheduleRepository
	{
		private readonly ApplicationDbContext context;

		public ScheduleRepository(ApplicationDbContext context) : base(context)
		{
			this.context = context;
		}


		public void Update(Schedule schedule)
		{
			context.Schedule.Update(schedule);
		}

	}
}
