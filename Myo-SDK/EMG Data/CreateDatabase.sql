/* 
*  FILE          : CreateDatabase.sql
*  PROJECT       : PROG3070 - Assignment #1
*  PROGRAMMER    : Cameron Huebner
*  FIRST VERSION : 2015-1-8
*  DESCRIPTION   : Creates/populates the CAP database for assignment 1    
*/

CREATE DATABASE EMGData;

Use EMGData;

CREATE TABLE EMG (
	first_emg INT NULL,
    second_emg INT NULL,
    third_emg INT NULL,
    forth_emg INT NULL,
    fifth_emg INT NULL,
    sixth_emg INT NULL,
    seventh_emg INT NULL,
    eighth_emg INT NULL,
    seconds INT NULL,
	PRIMARY KEY (first_emg)
);

/*---------------------------------------------------*/
INSERT INTO EMG (first_emg, second_emg, third_emg, forth_emg, fifth_emg, sixth_emg, seventh_emg, eighth_emg, seconds)
VALUES  ('0','0','0','0','0','0','0','0','0');
        
