using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public class ConfigIO : MonoBehaviour
{
    
    
    private string roboFilePath;
    private string goalFilePath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load(string path)
    {
        //get file paths out of the main file
        if (roboFilePath == null || goalFilePath == null)
        {
            throw new FileNotFoundException();
        }
        RoboRead(roboFilePath);
        GoalRead(goalFilePath);
    }

    private void RoboRead(string from)
    {
        using StreamReader rid = new(from);
        if (!int.TryParse(rid.ReadLine(), out int robn)) //
        {
            throw new InvalidDataException("The content of the file wasn't in the right format");
        }

        string temp = rid.ReadLine();
        while (temp != null)
        {
            if (robn <= 0)
            {
                throw new InvalidDataException("The content of the file wasn't in the right format, there were too many lines");
            }

            int.TryParse(rid.ReadLine(), out int newRobPos);
            //Robotmanager add
            robn--;
            temp = rid.ReadLine();
        }

    }    
    
    private void GoalRead(string path)
    {
        
    }
}
