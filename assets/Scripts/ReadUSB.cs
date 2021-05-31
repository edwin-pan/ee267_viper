﻿// This plugin read the data streaming from VRduino and attach the read
// quaternion values to the corresponding object.
//
// This plugin is initially written by Vasanth Mohan, and modified by
// Varsha Sankar and Sagar Honnungar to be compatible with Windows.
using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ReadUSB : MonoBehaviour
{
  const int baudrate = 115200;

  // Specify Correct Port Name
  // Change this to be compatible with your computer.
  // The easiest way to check the port anme is to check it on Arduino IDE.
  // For example, "COM4" on Windows and "/dev/tty.usbmodem2815011" on Mac.
  const string portName = "/dev/cu.usbmodem28150301";


    SerialPort serialPort = new SerialPort(portName, baudrate);

  void Start() {

    Debug.Log("Started");
    serialPort.ReadTimeout = 25;
    serialPort.Open();
    if (!serialPort.IsOpen) {
      Debug.LogError("Couldn't open " + portName);
    }
  }

  void Update() {

    List<byte> buffer = new List<byte>();

    // Read 64 bytes
    for (int i = 0; i < 64; i++) {
      buffer.Add((byte)serialPort.ReadByte());
    }

    // Convert list of bytes to string
    string text = System.Text.Encoding.UTF8.GetString(buffer.ToArray());

    // Split string into lines
    string[] lines = text.Split('\n');

    if (lines.Length >= 2) {
      // Split line by spaces
      string[] line = lines[lines.Length - 2].Split(' ');

      // If the line starts with "QC", parse the Quaternion
      if (line[0] == "QC") {
        Quaternion q =
            new Quaternion(-float.Parse(line[2]), float.Parse(line[3]),
                           float.Parse(line[4]), float.Parse(line[1]));

                Vector3 angles = Quaternion.Inverse(q).eulerAngles;
                transform.rotation = Quaternion.Euler(angles.x, angles.y, -angles.z); // to account for Unity's flipped coordinate system

      }
    }
  }

  void OnGUI() {
    string euler = "Euler angle: " + transform.eulerAngles.x + ", " +
                   transform.eulerAngles.y + ", " + transform.eulerAngles.z;
  }
}
