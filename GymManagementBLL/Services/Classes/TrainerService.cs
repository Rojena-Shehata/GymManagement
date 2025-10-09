using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public bool CreatTrainer(CreateTrainerViewModel model)
        {
            if(model is null) 
                return false;
            try
            {
                if (IsEmailExists(model.Email))
                    return false;
                if(IsPhoneExists(model.Phone))
                    return false;

                var trainer = new Trainer
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Specialties=model.Specialization,
                    Address=new Address
                    {
                        BuildingNumber=model.BuildingNumber,
                        Street = model.Street,
                        City = model.City
                    }
                };
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }


        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll() ?? [];
            if (trainers is null||!trainers.Any())
                return [];

            var trainerViewModel = trainers.Select(x => new TrainerViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Specialization=x.Specialties.ToString(),              
            });

             return trainerViewModel;
        }


        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer=_unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if(trainer is null)
                return null;
            var trainerViewModel = new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialization = trainer.Specialties.ToString(),
                Address=FormatedAddress(trainer.Address),
                DateOfBirth =trainer.DateOfBirth.ToShortDateString(),
            };
            return trainerViewModel;
        }


        public TrainerToUpdateViewModel? GetTrainerModelToUpdate(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if(trainer is null) 
                return null;
            var trainerViewModel = new TrainerToUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialization = trainer.Specialties,
            };

            return trainerViewModel;
        }


        public bool RemoveTrainer(int trainerId)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer= trainerRepo.GetById(trainerId);
            if(trainer is null)
                return false;
            try
            {

                var hasFutureSession = _unitOfWork.GetRepository<Session>()
                                               .Any(x=>x.TrainerId==trainerId&&x.StartDate>DateTime.Now);
                if (hasFutureSession)
                    return false;
                trainerRepo.Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTrainerData(int trainerId, TrainerToUpdateViewModel model)
        {
            if (model is null) 
                return false;
            var TrainerRepository = _unitOfWork.GetRepository<Trainer>();
            var trainer = TrainerRepository.GetById(trainerId);
            if(trainer is null)
                return false;
            try
            {
                if(trainer.Email != model.Email)
                {
                    if(IsEmailExists(model.Email))
                        return false;
                    trainer.Email = model.Email;                          
                }
                if(trainer.Phone != model.Phone)
                {
                    if(IsPhoneExists(model.Phone))
                        return false;
                    trainer.Phone = model.Phone;                          
                }
                trainer.Specialties = model.Specialization;
                trainer.Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City,
                };
                trainer.UpdatedAt = DateTime.Now;
                TrainerRepository.Update(trainer);
               return _unitOfWork.SaveChanges()>0;
            }
            catch
            {
                return false;
            }
        }






        #region Helper
        private string FormatedAddress(Address address)
        {
            if (address == null)
                return "N/A";
            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }

        private bool IsEmailExists(string email)
        {
            if (email == null) 
                return false;
           var IsExist=  _unitOfWork.GetRepository<Trainer>().Any(x=>x.Email== email);
            if (IsExist)
                return false;
            return true;
        }
        private bool IsPhoneExists(string phone)
        {
            if (phone == null) 
                return false;
           var IsExist=  _unitOfWork.GetRepository<Trainer>().Any(x=>x.Phone == phone);
            if (IsExist)
                return false;
            return true;
        }
        #endregion
    }
}
