//Arduio RadioManager Code
//Dan Berkowitz buildingtents.com
#include <Servo.h> 

Servo myservo;  // create servo object to control a servo 
// a maximum of eight servo objects can be created 
byte buttonPin = 2;    // select the input pin for the potentiometer
byte volume_pin = 5;   // Volume knob
byte station_knob = 2; 

void setup() {
  Serial.begin(9600);
  pinMode(buttonPin, INPUT);  
  pinMode(13, OUTPUT); 
  myservo.attach(9);  // attaches the servo on pin 9 to the servo object 
}

void loop() {
  digitalWrite(13, HIGH); //light
  byte data_length = 0;
  Serial.print('|'); data_length++;
  byte vol = volume_level(); 
  //There will be some code to follow to control packet size
  if (vol <= 0)
  {
    vol = 0;
    data_length++;
  }else{
    if(vol > 0 && vol < 10)
      {
        data_length++;
      }else{
        if(vol > 9 && vol < 100)
        {
          data_length += 2;
        }else
        {
          if(vol == 100)
            data_length += 3;
        }
      }
  }
  Serial.print(vol, DEC);
  Serial.print('|'); data_length++;
  byte channel = channel_knob();
  if (channel < 10)
    data_length++;
  else
    data_length += 2;
  Serial.print(channel,DEC);
  Serial.print('|'); data_length++;
  while( data_length < 11)
  {
    Serial.print("-");
    data_length++;
  }
  Serial.println();
  delay(200); //delay for a bit to rest

}

byte volume_level()
{
  int val = analogRead(volume_pin);
  //Serial.println(val, DEC);
  byte value = map(val, 0, 1023, 100, 0);
  return value;
}

byte channel_knob()
{
  int val = analogRead(station_knob);
  myservo.write( map(val, 0, 1023, 170, 0) );
  byte value = map(val, 0, 1023, 1, 10);
  return value;
}


