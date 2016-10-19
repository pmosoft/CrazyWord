using UnityEngine;
using System.Collections;
using System.IO;
using SQLite4Unity3d;
using System.Collections.Generic;



public class test01 : MonoBehaviour
{

    private ISQLiteConnection _connection;

    // Use this for initialization
    void Start()
    {

        var factory = new Factory();

        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", "existing.db");
        _connection = factory.Create(dbPath);
        Debug.Log("Final PATH: " + dbPath);


        var people = _connection.Table<Person>(); ;
        print("people=" + people.Count());

        foreach (Person p1 in people)
        {
            print("p1.Name=" + p1.Name);
        }



        //CreateDB2();

        string query = "update Person set Name='あいうぇえおか'";

        string val1 = "あいうぇえおか";
        //val1 = "aaa";

        query = string.Format("update Person set Name=\"{0}\" ", val1);



        //System.Text.Encoding.UTF8.GetByteCount(query);
        //_connection.Execute(query);
        //_connection.ExecuteScalar

        Person pp = _connection.Table<Person>().Where(x => x.Name == "あいうぇえおか").FirstOrDefault();

        print(pp.Age);

        _connection.Update(new Person{Id = 1,Name = "탐",Surname = "あいうぇえおか",Age = 56});

        //_connection.Execute("update Person set Name='abc'");


    }

    public class imsi02
    {

        public string Name { get; set; }

    }


    public void CreateDB2()
    {

        _connection.InsertAll(new[]{
			new imsi02{
				Name = "탐",
			},
			new imsi02{
				Name = "あいうぇえおか",
			}
		});
    }


    public void CreateDB()
    {
        _connection.DropTable<Person>();
        _connection.CreateTable<Person>();

        _connection.InsertAll(new[]{
			new Person{
				Id = 1,
				Name = "탐",
				Surname = "Perez",
				Age = 56
			},
			new Person{
				Id = 2,
				Name = "あいうぇえおか",
				Surname = "Arthurson",
				Age = 16
			},
			new Person{
				Id = 3,
				Name = "John",
				Surname = "Doe",
				Age = 25
			},
			new Person{
				Id = 4,
				Name = "Roberto",
				Surname = "Huertas",
				Age = 37
			},
		});
    }

}