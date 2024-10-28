using Clinical_Management_System.IRepositery;
using Clinical_Management_System.Models.DB_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical_Management_System.Repository.IRepositery
{
	public interface ISpecializtionRepository : IRepository<Specialization>
	{
		void Update(Specialization specialization);

	}
}
