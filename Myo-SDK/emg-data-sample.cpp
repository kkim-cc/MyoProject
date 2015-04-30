// Copyright (C) 2013-2014 Thalmic Labs Inc.
// Distributed under the Myo SDK license agreement. See LICENSE.txt for details.

// This sample illustrates how to use EMG data. EMG streaming is only supported for one Myo at a time.

#include <array>
#include <iostream>
#include <sstream>
#include <stdexcept>
#include <string>
#include <fstream>
#include <ctime>
#include <Windows.h>

#include <myo/myo.hpp>

class DataCollector : public myo::DeviceListener {
	HANDLE hPipe1, hPipe2;
	//Pipe Init Data
	char buf[100];
	LPTSTR lpszPipename1 = TEXT("\\\\.\\pipe\\myNamedPipe1");
	LPTSTR lpszPipename2 = TEXT("\\\\.\\pipe\\myNamedPipe2");
	DWORD cbWritten;
	DWORD dwBytesToWrite=100;

public:
    DataCollector()
    : emgSamples()
    {
    }

	// initialize the named pipe
	bool initializeNamedPipe()
	{
		hPipe1 = CreateFile(lpszPipename1, GENERIC_WRITE, 0, NULL, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, NULL);
		hPipe2 = CreateFile(lpszPipename2, GENERIC_WRITE, 0, NULL, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, NULL);
		if (hPipe1 == NULL || hPipe1 == INVALID_HANDLE_VALUE || hPipe2 == NULL || hPipe2 == INVALID_HANDLE_VALUE)
		{
			std::cout << "Could not open the pipe - " << GetLastError() << std::endl;
			return false;
		}

		memset(buf, 0xCC, 100);

		return true;
	}

	// close the named pipe
	void closeNamedPipe()
	{
		CloseHandle(hPipe1);
		CloseHandle(hPipe2);
	}

    // onUnpair() is called whenever the Myo is disconnected from Myo Connect by the user.
    void onUnpair(myo::Myo* myo, uint64_t timestamp)
    {
        // We've lost a Myo.
        // Let's clean up some leftover state.
        emgSamples.fill(0);
    }

    // onEmgData() is called whenever a paired Myo has provided new EMG data, and EMG streaming is enabled.
    void onEmgData(myo::Myo* myo, uint64_t timestamp, const int8_t* emg)
    {
        for (int i = 0; i < 8; i++) {
            emgSamples[i] = emg[i];
        }
    }

    // There are other virtual functions in DeviceListener that we could override here, like onAccelerometerData().
    // For this example, the functions overridden above are sufficient.

    // We define this function to print the current values that were updated by the on...() functions above.
    void print()
    {
		std::string fileout;
        // Clear the current line
        std::cout << '\r';
        // Print out the EMG data.

		std::clock_t    start;

		start = std::clock();

        for (size_t i = 0; i < emgSamples.size(); i++) {
            std::ostringstream oss;
            oss << static_cast<int>(emgSamples[i]);
            std::string emgString = oss.str();
            std::cout << '[' << emgString << std::string(4 - emgString.size(), ' ') << ']';
			fileout = fileout + emgString + " , " ;
        }
				
		fileout = fileout + std::to_string(std::clock()); //+ "ms";

		// Write data to Named Pipe!
		// hPipe 1 is for Ghart display
		// hPipe 2 is for Graphic display
		strcpy_s(buf, fileout.c_str());
		WriteFile(hPipe1, buf, dwBytesToWrite, &cbWritten, NULL);
		WriteFile(hPipe2, buf, dwBytesToWrite, &cbWritten, NULL);
		memset(buf, 0xCC, 100);

		// first work -- save EMG data to local txt file.
		// std::ofstream myfile;
		// myfile.open ("emgdata.txt", std::ios_base::app);
		// myfile <<  fileout + "\n";
		// myfile.close();
        // std::cout << std::flush;
    }

    // The values of this array is set by onEmgData() above.
    std::array<int8_t, 8> emgSamples;
};

int main(int argc, char** argv)
{
    // We catch any exceptions that might occur below -- see the catch statement for more details.
    try {

    // First, we create a Hub with our application identifier. Be sure not to use the com.example namespace when
    // publishing your application. The Hub provides access to one or more Myos.
    myo::Hub hub("com.example.emg-data-sample");

    std::cout << "Attempting to find a Myo..." << std::endl;

    // Next, we attempt to find a Myo to use. If a Myo is already paired in Myo Connect, this will return that Myo
    // immediately.
    // waitForMyo() takes a timeout value in milliseconds. In this case we will try to find a Myo for 10 seconds, and
    // if that fails, the function will return a null pointer.
    myo::Myo* myo = hub.waitForMyo(10000);

    // If waitForMyo() returned a null pointer, we failed to find a Myo, so exit with an error message.
    if (!myo) {
        throw std::runtime_error("Unable to find a Myo!");
    }

    // We've found a Myo.
    std::cout << "Connected to a Myo armband!" << std::endl << std::endl;

    // Next we enable EMG streaming on the found Myo.
    myo->setStreamEmg(myo::Myo::streamEmgEnabled);

    // Next we construct an instance of our DeviceListener, so that we can register it with the Hub.
    DataCollector collector;

    // Hub::addListener() takes the address of any object whose class inherits from DeviceListener, and will cause
    // Hub::run() to send events to all registered device listeners.
    hub.addListener(&collector);

	if (collector.initializeNamedPipe() == false)
	{
		return 1;
	}

    // Finally we enter our main loop.
    while (1) {
        // In each iteration of our main loop, we run the Myo event loop for a set number of milliseconds.
        // In this case, we wish to update our display 50 times a second, so we run for 1000/20 milliseconds.
        hub.run(1000/20);
        // After processing events, we call the print() member function we defined above to print out the values we've
        // obtained from any events that have occurred.
        collector.print();
    }

	collector.closeNamedPipe();

    // If a standard exception occurred, we print out its message and exit.
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        std::cerr << "Press enter to continue.";
        std::cin.ignore();
        return 1;
    }
	return 0;
}
