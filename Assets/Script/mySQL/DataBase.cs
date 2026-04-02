using UnityEngine;
using MySql.Data.MySqlClient;

public class MySqlTest : MonoBehaviour
{
    string str = "Server=localhost;Database=DB0;Uid=root;Pwd=000000;SslMode=None;";

    void SerachData()
    {
        using (MySqlConnection conn = new MySqlConnection(str))
        {
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Users", conn);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read()) 
                {
                    string name = rdr["Name"].ToString();
                    string age = rdr["Age"].ToString();
                    Debug.Log("[조회 성공] name:" + name + "age:" + age);
                }
            }
        }
    }
    
    void InsertData(MySqlConnection target, string name, int age)
    {
        string sql = $"INSERT INTO Users (Name, Age) VALUES ('{name}', {age})";
    
        MySqlCommand cmd = new MySqlCommand(sql, target);
        cmd.ExecuteNonQuery();
        
        Debug.Log(name + "/" + age + "추가됨");
    }
    
    void UpdateData(MySqlConnection target, string name, int newAge)
    {
        string temp = $"UPDATE Users SET Age = {newAge} WHERE Name = '{name}'";
    
        MySqlCommand cmd = new MySqlCommand(temp, target);
        int result = cmd.ExecuteNonQuery();
    }

    void DeleteData(MySqlConnection target, string name)
    {
        string temp = $"DELETE FROM Users WHERE Name = '{name}'";
    
        MySqlCommand cmd = new MySqlCommand(temp, target);
        int result = cmd.ExecuteNonQuery();
    }
    
    void Start()
    {
        using (MySqlConnection conn = new MySqlConnection(str))
        {
            conn.Open();

            InsertData(conn,"김정호",21);

            SerachData();
            
            UpdateData(conn,"김정호",55);
            
            Debug.Log("수정된 후:");
            SerachData();
            
            DeleteData(conn,"김정호");
            
            Debug.Log("지운 후:");
            SerachData();
        }
    }
}