using Clinical_Management_System.Data;
using Clinical_Management_System.Repository.IRepositery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical_Management_System.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext context;
		public IAppointmentRepository appointmentRepository { get; private set; }

		public IScheduleRepository scheduleRepository { get; private set; }

		public IPrescriptionRepository prescriptionRepository { get; private set; }

		public IClinicRepository clinicRepository { get; private set; }

		public IMedicineRepository mediicineRepository { get; private set; }

		public IDocumentRepository documentRepository { get; private set; }

		public ISpecializtionRepository specializtionRepository { get; private set; }

		public UnitOfWork(ApplicationDbContext context)
		{
			this.context = context;
			appointmentRepository = new AppointmentRepository(context);
			scheduleRepository= new ScheduleRepository(context);

			prescriptionRepository = new PrescriptionRepository(context);
			clinicRepository =new ClinicRepository(context);
			mediicineRepository =new MedicineRepository(context);
			documentRepository = new DocumentRepository(context);
			specializtionRepository =new SpecializtionRepository(context);
		}
		public void Save()
		{
			context.SaveChanges();
		}

	}
}
