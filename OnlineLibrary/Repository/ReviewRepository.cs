using OnlineLibrary.Data;
using OnlineLibrary.Models.DBObjects;
using OnlineLibrary.Models;

namespace OnlineLibrary.Repository
{
    public class ReviewRepository
    {
        private ApplicationDbContext dbContext;
        public ReviewRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }
        public ReviewRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<ReviewModel> GetReviews()
        {
            List<ReviewModel> reviewsList = new List<ReviewModel>();
            foreach (Review review in dbContext.Reviews)
            {
                reviewsList.Add(MapDbObjectToModel(review));
            }
            return reviewsList;
        }
        public ReviewModel GetReviewByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Reviews.FirstOrDefault(x => x.Idreview == ID));
        }
        public List<ReviewModel> GetReviewByMember(string memberID)
        {
            List<ReviewModel> reviewsList = new List<ReviewModel>();

            foreach (Review review in dbContext.Reviews.Where(x => x.Idmember.Equals (memberID)))
            {
                reviewsList.Add(MapDbObjectToModel(review));
            }
            return reviewsList;
        }
        public List<ReviewModel> GetReviewByBook(Guid bookID)
        {
            List<ReviewModel> reviewsList = new List<ReviewModel>();

            foreach (Review review in dbContext.Reviews.Where(x => x.Idbook == bookID))
            {
                reviewsList.Add(MapDbObjectToModel(review));
            }
            return reviewsList;
        }
        public void InsertReview(ReviewModel reviewModel)
        {
            reviewModel.Idreview = Guid.NewGuid();
            dbContext.Reviews.Add(MapModelToDbObject(reviewModel));
            dbContext.SaveChanges();
        }

        public void UpdateReview(ReviewModel reviewModel)
        {
            Review existingReview = dbContext.Reviews.FirstOrDefault(x => x.Idreview == reviewModel.Idreview);
            if (existingReview != null)
            {
                existingReview.Idreview = reviewModel.Idreview;
                existingReview.Idmember = reviewModel.Idmember;
                existingReview.Idbook = reviewModel.Idbook;
                existingReview.Rating = reviewModel.Rating;
                existingReview.Text = reviewModel.Text;
                reviewModel.Date = reviewModel.Date;
                dbContext.SaveChanges();
            }
        }
        public void DeleteReview(Guid id)
        {
            Review existingReview = dbContext.Reviews.FirstOrDefault(x => x.Idreview == id);
            if (existingReview != null)
            {
                dbContext.Reviews.Remove(existingReview);
                dbContext.SaveChanges();
            }
        }
        private ReviewModel MapDbObjectToModel(Review dbReview)
        {
            ReviewModel reviewModel = new ReviewModel();
            if (dbReview != null)
            {
                reviewModel.Idreview = dbReview.Idreview;
                reviewModel.Idmember = dbReview.Idmember.ToString();
                reviewModel.Idbook = dbReview.Idbook;
                reviewModel.Rating = dbReview.Rating;
                reviewModel.Text = dbReview.Text;
                reviewModel.Date = dbReview.Date;
            }
            return reviewModel;
        }
        private Review MapModelToDbObject(ReviewModel reviewModel)
        {
            Review review = new Review();
            if (reviewModel != null)
            {
                review.Idreview = reviewModel.Idreview;
                review.Idmember = reviewModel.Idmember;
                review.Idbook = reviewModel.Idbook;
                review.Rating = reviewModel.Rating;
                review.Text = reviewModel.Text;
                review.Date = reviewModel.Date;
            }
            return review;
        }
    }
}
