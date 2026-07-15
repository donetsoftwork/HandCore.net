using BenchmarkDotNet.Running;
using ParseJsonBench;

#if DEBUG
UserSingleBench userSingle = new();
var userSingle1 = userSingle.Deserialize();
var userSingle2 = userSingle.GetResult();
var userSingle3 = userSingle.GetResult1();
var userSingle4 = userSingle.GetResult2();
var userSingle5 = userSingle.GetResult3();
var userSingle6 = userSingle.Custom();
UserListBench userList = new();
var userList1 = userList.Deserialize();
var userList2 = userList.GetResult();
var userList3 = userList.GetResult1();
var userList4 = userList.GetResult2();
var userList5 = userList.GetResult3();
var userList6 = userList.Custom();
Console.ReadLine();

#else
//BenchmarkRunner.Run<UserSingleBench>();
BenchmarkRunner.Run<UserListBench>();
#endif