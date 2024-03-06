#include <8052.h>

void delay(void);
 
void main(void)
{
    while(1)
    {
        P1_0 = 0x00;
        delay();
        P1_0 = 0x01;
        delay();
    }
}

void delay(void)
{
    int i, j;
    for(i = 0; i < 0xFF; i++)
        for(j = 0; j < 0xFF; j++);
}
