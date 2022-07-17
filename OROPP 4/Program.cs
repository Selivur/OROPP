﻿Semaphore sem = new Semaphore(2, 2);
int sum=0;
for (int i = 0; i < 6; i++)
{
    var cust = new Customer(i);
    new Thread(() => Cafe(cust, ref sum)).Start();
}
Thread.Sleep(3000);
Console.WriteLine($"You earned {sum}$");
void Cafe(Customer customer, ref int sum)
{
        sem.WaitOne();
        Console.WriteLine($"Customer Number {customer.myId} entered to the cafe ");
        Thread.Sleep(200);
        sum = sum + customer.sum;
        Console.WriteLine($"Customer Number {customer.myId} paid {customer.sum} and go out");
        sem.Release();
}
class Customer
{    
    public int myId { get; set; }
    public int sum { get; set; }
    Random rnd = new Random();
    public Customer(int i)
    {
        myId = i;
        sum = rnd.Next(100);
    }
}
