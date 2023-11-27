using OnlineLibrary.Data;
using OnlineLibrary.Models.DBObjects;
using OnlineLibrary.Models;

namespace OnlineLibrary.Repository
{
    public class MemberRepository
    {
        private ApplicationDbContext dbContext;
        public MemberRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }
        public MemberRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<MemberModel> GetMembers()
        {
            List<MemberModel> membersList = new List<MemberModel>();
            foreach (Member member in dbContext.Members)
            {
                membersList.Add(MapDbObjectToModel(member));
            }
            return membersList;
        }
        public MemberModel GetMemberByID(string ID)
        {
            return MapDbObjectToModel(dbContext.Members.FirstOrDefault(x => x.Idmember.Equals( ID)));
        }
        public List<MemberModel> GetMemberByRegistrationDate(DateTime date)
        {
            List<MemberModel> membersList = new List<MemberModel>();

            foreach (Member member in dbContext.Members.Where(x => x.RegistrationDate == date))
            {
                membersList.Add(MapDbObjectToModel(member));
            }
            return membersList;
        }
        public void InsertMember(MemberModel memberModel)
        {
            memberModel.Idmember = Guid.NewGuid().ToString();
            dbContext.Members.Add(MapModelToDbObject(memberModel));
            dbContext.SaveChanges();
        }

        public void UpdateMember(MemberModel memberModel)
        {
            Member existingMember = dbContext.Members.FirstOrDefault(x => x.Idmember == memberModel.Idmember);
            if (existingMember != null)
            {
                existingMember.Idmember = memberModel.Idmember;
                existingMember.Username = memberModel.Username;
                existingMember.Password = memberModel.Password;
                existingMember.Email = memberModel.Email;
                existingMember.RegistrationDate = memberModel.RegistrationDate;
                dbContext.SaveChanges();
            }
        }
        public void DeleteMember(string id)
        {
            Member existingMember = dbContext.Members.FirstOrDefault(x => x.Idmember.Equals( id));
            if (existingMember != null)
            {
                dbContext.Members.Remove(existingMember);
                dbContext.SaveChanges();
            }
        }
        private MemberModel MapDbObjectToModel(Member dbMember)
        {
            MemberModel memberModel = new MemberModel();
            if (dbMember != null)
            {
                memberModel.Idmember = dbMember.Idmember;
                memberModel.Username = dbMember.Username;
                memberModel.Password = dbMember.Password;
                memberModel.Email = dbMember.Email;
                memberModel.RegistrationDate = dbMember.RegistrationDate;
            }
            return memberModel;
        }
        private Member MapModelToDbObject(MemberModel memberModel)
        {
            Member member = new Member();
            if (memberModel != null)
            {
                member.Idmember = memberModel.Idmember;
                member.Username = memberModel.Username;
                member.Password = memberModel.Password;
                member.Email = memberModel.Email;
                member.RegistrationDate = memberModel.RegistrationDate;
            }
            return member;
        }

    }
}
