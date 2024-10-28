using Clinical_Management_System.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical_Management_System.Repository.IRepositery
{
	public interface IUnitOfWork
	{
		IAppointmentRepository appointmentRepository { get; }

		IScheduleRepository scheduleRepository { get; }

		IPrescriptionRepository prescriptionRepository { get; }

		IClinicRepository clinicRepository { get; }

		IMedicineRepository mediicineRepository { get; }

		IDocumentRepository documentRepository { get; }

		ISpecializtionRepository specializtionRepository { get; }
		void Save();
	}
}
