using OnlineLibrary.Data;
using OnlineLibrary.Models.DBObjects;
using OnlineLibrary.Models;

namespace OnlineLibrary.Repository
{
    public class FeeRepository
    {
        private ApplicationDbContext dbContext;
        public FeeRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }
        public FeeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<FeeModel> GetFees()
        {
            List<FeeModel> feesList = new List<FeeModel>();
            foreach (Fee fee in dbContext.Fees)
            {
                feesList.Add(MapDbObjectToModel(fee));
            }
            return feesList;
        }
        public FeeModel GetFeeByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Fees.FirstOrDefault(x => x.Idfee == ID));
        }
        public List<FeeModel> GetFeeByBook(Guid bookID)
        {
            List<FeeModel> feesList = new List<FeeModel>();

            foreach (Fee fee in dbContext.Fees.Where(x => x.Idbook == bookID))
            {
                feesList.Add(MapDbObjectToModel(fee));
            }
            return feesList;
        }
        public List<FeeModel> GetFeeByMember(string memberID)
        {
            List<FeeModel> feesList = new List<FeeModel>();

            foreach (Fee fee in dbContext.Fees.Where(x => x.Idmember.Equals(memberID)))
            {
                feesList.Add(MapDbObjectToModel(fee));
            }
            return feesList;
        }
        public List<FeeModel> GetFeeByPaidStatus(int isPaid)
        {
            List<FeeModel> feesList = new List<FeeModel>();

            foreach (Fee fee in dbContext.Fees.Where(x => x.IsPaid == isPaid))
            {
                feesList.Add(MapDbObjectToModel(fee));
            }
            return feesList;
        }
        public List<FeeModel> GetFeesSmaller(decimal amount)
        {
            List<FeeModel> feesList = new List<FeeModel>();

            foreach (Fee fee in dbContext.Fees.Where(x => x.Amount < amount))
            {
                feesList.Add(MapDbObjectToModel(fee));
            }
            return feesList;
        }
        public List<FeeModel> GetFeesEqual(decimal amount)
        {
            List<FeeModel> feesList = new List<FeeModel>();

            foreach (Fee fee in dbContext.Fees.Where(x => x.Amount == amount))
            {
                feesList.Add(MapDbObjectToModel(fee));
            }
            return feesList;
        }
        public List<FeeModel> GetFeesBigger(decimal amount)
        {
            List<FeeModel> feesList = new List<FeeModel>();

            foreach (Fee fee in dbContext.Fees.Where(x => x.Amount > amount))
            {
                feesList.Add(MapDbObjectToModel(fee));
            }
            return feesList;
        }
        public void InsertFee(FeeModel feeModel)
        {
            feeModel.Idfee = Guid.NewGuid();
            dbContext.Fees.Add(MapModelToDbObject(feeModel));
            dbContext.SaveChanges();
        }

        public void UpdateFee(FeeModel feeModel)
        {
            Fee existingFee = dbContext.Fees.FirstOrDefault(x => x.Idfee == feeModel.Idfee);
            if (existingFee != null)
            {
                existingFee.Idfee = feeModel.Idfee;
                existingFee.Idbook = feeModel.Idbook;
                existingFee.Idmember = feeModel.Idmember;
                existingFee.Description = feeModel.Description;
                existingFee.IsPaid = feeModel.IsPaid;
                existingFee.Amount = feeModel.Amount;
                dbContext.SaveChanges();
            }
        }
        public void DeleteFee(Guid id)
        {
            Fee existingFee = dbContext.Fees.FirstOrDefault(x => x.Idfee == id);
            if (existingFee != null)
            {
                dbContext.Fees.Remove(existingFee);
                dbContext.SaveChanges();
            }
        }
        private FeeModel MapDbObjectToModel(Fee dbFee)
        {
            FeeModel feeModel = new FeeModel();
            if (dbFee != null)
            {
                feeModel.Idfee = dbFee.Idfee;
                feeModel.Idbook = dbFee.Idbook;
                feeModel.Idmember = dbFee.Idmember;
                feeModel.Description = dbFee.Description;
                feeModel.IsPaid = dbFee.IsPaid;
                feeModel.Amount = dbFee.Amount;
            }
            return feeModel;
        }
        private Fee MapModelToDbObject(FeeModel feeModel)
        {
            Fee fee = new Fee();
            if (feeModel != null)
            {
                fee.Idfee = feeModel.Idfee;
                fee.Idbook = feeModel.Idbook;
                fee.Idmember = feeModel.Idmember;
                fee.Description = feeModel.Description;
                fee.IsPaid = feeModel.IsPaid;
                fee.Amount = feeModel.Amount;
            }
            return fee;
        }
    }
}
