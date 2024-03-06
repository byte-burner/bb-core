#!/bin/bash


# FT232H
FT232H_VendorId=0403
FT232H_ProductId=6014
FT232H_FILE="/etc/udev/rules.d/99-FT232H.rules"
FT232H_RULE="ACTION==\"add\", ATTRS{idVendor}==\"$FT232H_VendorId\", ATTRS{idProduct}==\"$FT232H_ProductId\", MODE=\"0666\",  RUN+=\"/bin/sh -c 'rmmod ftdi_sio && rmmod usbserial'\""
echo "$FT232H_RULE" | sudo tee -a $FT232H_FILE

# add more rules for other devices here


# Reload udev rules
sudo udevadm control --reload-rules
sudo udevadm trigger