using OnlineLibrary.Data;
using OnlineLibrary.Models;
using OnlineLibrary.Models.DBObjects;

namespace OnlineLibrary.Repository
{
    public class TransactionRepository
    {
        private ApplicationDbContext dbContext;
        public TransactionRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }
        public TransactionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<TransactionModel> GetTransactions()
        {
            List<TransactionModel> transactionsList = new List<TransactionModel>();
            foreach (Transaction transaction in dbContext.Transactions)
            {
                transactionsList.Add(MapDbObjectToModel(transaction));
            }
            return transactionsList;
        }
        public TransactionModel GetTransactionByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Transactions.FirstOrDefault(x => x.Idtransaction == ID));
        }
        public List<TransactionModel> GetTransactionByDate(DateTime date)
        {
            List<TransactionModel> transactionsList = new List<TransactionModel>();

            foreach (Transaction transaction in dbContext.Transactions.Where(x => x.Date == date))
            {
                transactionsList.Add(MapDbObjectToModel(transaction));
            }
            return transactionsList;
        }
        public List<TransactionModel> GetTransactionByReturn(DateTime retur)
        {
            List<TransactionModel> transactionsList = new List<TransactionModel>();

            foreach (Transaction transaction in dbContext.Transactions.Where(x => x.Retrun == retur))
            {
                transactionsList.Add(MapDbObjectToModel(transaction));
            }
            return transactionsList;
        }
        public List<TransactionModel> GetTransactionByMember(string memberID)
        {
            List<TransactionModel> transactionsList = new List<TransactionModel>();

            foreach (Transaction transaction in dbContext.Transactions.Where(x => string.Equals(x.Idmember, memberID, StringComparison.OrdinalIgnoreCase)))
            {
                transactionsList.Add(MapDbObjectToModel(transaction));
            }

            return transactionsList;
        }

        public List<TransactionModel> GetTransactionByBook(Guid bookID)
        {
            List<TransactionModel> transactionsList = new List<TransactionModel>();

            foreach (Transaction transaction in dbContext.Transactions.Where(x => x.Idbook == bookID))
            {
                transactionsList.Add(MapDbObjectToModel(transaction));
            }
            return transactionsList;
        }
        public List<TransactionModel> GetTransactionByStatus(string status)
        {
            List<TransactionModel> transactionsList = new List<TransactionModel>();

            foreach (Transaction transaction in dbContext.Transactions.Where(x => x.Status.Equals(status)))
            {
                transactionsList.Add(MapDbObjectToModel(transaction));
            }
            return transactionsList;
        }
        public void InsertTransaction(TransactionModel transactionModel)
        {
            // Ensure that the book with the specified ID exists
            var book = dbContext.Books.FirstOrDefault(b => b.Idbook == transactionModel.Idbook);

            if (book != null && book.AvailableCopies > 0)
            {
                transactionModel.Idtransaction = Guid.NewGuid();
                transactionModel.Retrun = transactionModel.Date.AddDays(30); // Set return date as Date + 30 days

                // Decrement the AvailableCopies
                book.AvailableCopies--;

                dbContext.Transactions.Add(MapModelToDbObject(transactionModel));
                dbContext.SaveChanges();
            }
            // Optionally, you can add an else statement to handle the case where the book is not available.
        }




        public void UpdateTransaction(TransactionModel transactionModel)
        {
            Transaction existingTransaction = dbContext.Transactions.FirstOrDefault(x => x.Idtransaction == transactionModel.Idtransaction);
            if (existingTransaction != null)
            {
                existingTransaction.Idtransaction = transactionModel.Idtransaction;
                existingTransaction.Idmember = transactionModel.Idmember;
                existingTransaction.Idbook = transactionModel.Idbook;
                existingTransaction.Date = transactionModel.Date;
                existingTransaction.Retrun = transactionModel.Retrun;
                existingTransaction.Status = transactionModel.Status;
                dbContext.SaveChanges();
            }
        }
        public void DeleteTransaction(Guid id)
        {
            Transaction existingTransaction = dbContext.Transactions.FirstOrDefault(x => x.Idtransaction == id);
            if (existingTransaction != null)
            {
                dbContext.Transactions.Remove(existingTransaction);
                dbContext.SaveChanges();
            }
        }
        private TransactionModel MapDbObjectToModel(Transaction dbTransaction)
        {
            TransactionModel transactionModel = new TransactionModel();
            if (dbTransaction != null)
            {
                transactionModel.Idtransaction = dbTransaction.Idtransaction;
                transactionModel.Idmember = dbTransaction.Idmember;
                transactionModel.Idbook = dbTransaction.Idbook;
                transactionModel.Date = dbTransaction.Date;
                transactionModel.Retrun = dbTransaction.Retrun;
                transactionModel.Status = dbTransaction.Status;
            }
            return transactionModel;
        }
        private Transaction MapModelToDbObject(TransactionModel transactionModel)
        {
            Transaction transaction = new Transaction();
            if (transactionModel != null)
            {
                transaction.Idtransaction = transactionModel.Idtransaction;
                transaction.Idmember = transactionModel.Idmember;
                transaction.Idbook = transactionModel.Idbook;
                transaction.Date = transactionModel.Date;
                transaction.Retrun = transactionModel.Retrun;
                transaction.Status = transactionModel.Status;
            }
            return transaction;
        }
    }
}
