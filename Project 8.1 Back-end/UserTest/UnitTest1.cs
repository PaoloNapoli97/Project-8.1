using UserServices;
using UserModel;
using LabModel;
using LabApi.Controllers;
using ComputerDTO;
using BookingApi.Controllers;

namespace Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestReadUsers()
    {
        UserService userService = new UserService();
        List<User> users = userService.ReadUsers();
        Assert.IsNotNull(users);
    }

    [TestMethod]
    public void TestAddBooking()
    {
        Computer computer = new Computer("TestComputer", "TestDescription", "TestSpecs", Computer.StatusList.Avaiable, DateTime.Now);
        string testUserId = "testId";
        string testDay = "Monday";
        string testHour = "9:00";

        computer.AddBooking(testUserId, testDay, testHour);

        var bookingsForTheDay = computer.Calendar.Where(x => x.Key.Contains(testDay) && x.Value == testUserId);
        Assert.IsTrue(bookingsForTheDay.Count() == 1);
    }

    [TestMethod]
    public void TestDeleteBooking()
    {
        Computer computer = new Computer("TestComputer", "TestDescription", "TestSpecs", Computer.StatusList.Avaiable, DateTime.Now);
        string testUserId = "testId";
        string testDay = "Monday";
        string testHour = "9:00";

        computer.AddBooking(testUserId, testDay, testHour);
        computer.DeleteBooking(testUserId, testDay, testHour);

        var bookingsForTheDay = computer.Calendar.Where(x => x.Key.Contains(testDay) && x.Value == testUserId);
        Assert.IsTrue(bookingsForTheDay.Count() == 0);
    }
}