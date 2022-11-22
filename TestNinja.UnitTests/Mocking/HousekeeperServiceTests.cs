using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HousekeeperServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _xtraMessageBox;
        private Housekeeper _housekeeper;
        private HousekeeperService _housekeeperService;
        private DateTime _statementDate = new DateTime(2022, 10, 10);
        private string _statementFileName;

        [SetUp]
        public void SetUp()
        {
            _housekeeper = new Housekeeper
            {
                Email = "a",
                Oid = 1,
                FullName = "b",
                StatementEmailBody = "c"
            };

            this._unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {
                _housekeeper
            }.AsQueryable());

            _statementFileName = "fileName";
            this._statementGenerator = new Mock<IStatementGenerator>();
            _statementGenerator
                .Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
                .Returns(() => _statementFileName);

            this._emailSender = new Mock<IEmailSender>();


            this._xtraMessageBox = new Mock<IXtraMessageBox>();
            this._housekeeperService = new HousekeeperService(_unitOfWork.Object,
                                                              _statementGenerator.Object,
                                                              _emailSender.Object,
                                                              _xtraMessageBox.Object);
        }

        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {
            _housekeeperService.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate));
        }

        [Test]
        public void SendStatementEmails_HouseKeepersEmailIsNull_ShouldNotGenerateStatement()
        {
            _housekeeper.Email = null;

            _housekeeperService.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate), Times.Never);
        }
        [Test]
        public void SendStatementEmails_HouseKeepersEmailIsEmpty_ShouldNotGenerateStatement()
        {
            _housekeeper.Email = "";

            _housekeeperService.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate), Times.Never);
        }

        [Test]
        public void SendStatementEmails_HouseKeepersEmailIsWhiteSpace_ShouldNotGenerateStatement()
        {
            _housekeeper.Email = " ";

            _housekeeperService.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate), Times.Never);
        }

        [Test]
        public void SendStatementEmails_WhenCalled_EmailStatement()
        {
            _housekeeperService.SendStatementEmails(_statementDate);

            this.VerifyEmailSent();
        }


        [Test]
        public void SendStatementEmails_StatementFileNameIsNull_ShouldNotEmailTheStatement()
        {
            _statementFileName = null;

            _housekeeperService.SendStatementEmails(_statementDate);

            this.VerifyEmailNotSent();
        }

        [Test]
        public void SendStatementEmails_StatementFileNameIsEmpty_ShouldNotEmailTheStatement()
        {
            _statementFileName = "";

            _housekeeperService.SendStatementEmails(_statementDate);

            this.VerifyEmailNotSent();
        }

        [Test]
        public void SendStatementEmails_StatementFileNameIsWhiteSpace_ShouldNotEmailTheStatement()
        {
            _statementFileName = " ";

            _housekeeperService.SendStatementEmails(_statementDate);

            this.VerifyEmailNotSent();
        }

        public void SendStatementEmails_EmailFails_DisplayAMessageBox()
        {
            _emailSender.Setup(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Throws<Exception>();

            _housekeeperService.SendStatementEmails(_statementDate);

            this.VerifyDisplayMessageBox();
        }

        private void VerifyEmailNotSent()
        {
            _emailSender.Verify(es => es.EmailFile(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Never);
        }

        private void VerifyEmailSent()
        {
            _emailSender.Verify(es => es.EmailFile(
                    _housekeeper.Email,
                    _housekeeper.StatementEmailBody,
                    _statementFileName,
                    It.IsAny<string>()));
        }

        private void VerifyDisplayMessageBox()
        {
            _xtraMessageBox.Verify(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButtons.OK));
        }
    }
}
