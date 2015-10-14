using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompoundInterest
{
    class Program
    {

        public const int loanTerm = 36;
        public const int months = 12;

        public static void Main()
        {
            int RequestedAmount = 0;
            Console.WriteLine("Please enter the amount you would like to borrow");
            RequestedAmount = Convert.ToInt32(Console.ReadLine());
            var path = @"C:\Users\tom.groombridge\downloads\market_no_headers.csv";
            if (File.Exists(path))
            {
                 EnoughInPot(path, RequestedAmount);               
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", path);
            }
            
        }
        

        public static int EnoughInPot(string path, int RequestedAmount)
        {
            string[][] lenderData = File.ReadLines(path)
                          .Select(line => line.Split(','))
                          .ToArray();
            var orderedList = lenderData.OrderBy(entry => entry[1]).ToArray();

            List<int> amountAvailable = new List<int>();
            List<decimal> interestAvailable = new List<decimal>();
            foreach (var item in orderedList)
            {                
                var decValue = int.Parse(item[2]);
                var interestValue = decimal.Parse(item[1]);                              
                interestAvailable.Add(interestValue);
                amountAvailable.Add(decValue);
            }            

            if (amountAvailable.Sum() > RequestedAmount)
            {
                var lenderAmounts = amountAvailable;
                int sum = 0;
                int i = 0;                
                do
                {
                    sum += lenderAmounts[i];
                    i++;
                } while (sum < RequestedAmount);

                decimal allInterest = 0;
                int x = 0;
                do
                {
                    allInterest += interestAvailable[x];
                    x++;
                } while (x < i);
                decimal compInterest = allInterest / i;
                decimal principalPayment = ((decimal)RequestedAmount / loanTerm);
                Array LendersNeeded = orderedList.Take(i).ToArray();
                SetRepayments(compInterest, principalPayment);
                return i;
            }
            else
            {
                Console.WriteLine("Sorry but we do not have enough funds for your loan");
                return 0;
            }
        }

        public static int SetRepayments(decimal rate, decimal principal)
        {            
            decimal totalRepayments = 0;
            decimal remainingPrincipal = 1000;           
            do
            {
                decimal interestPerMonth = ((remainingPrincipal * Math.Round(rate, 2)) / months);                
                totalRepayments += (principal + Math.Round(interestPerMonth, 2));
                remainingPrincipal = (remainingPrincipal - principal);
               
            } while (remainingPrincipal >= 0);
            decimal monthlyRepayments = (totalRepayments / loanTerm);
            totalRepayments = (monthlyRepayments * loanTerm);
            Console.WriteLine("total repayments " + totalRepayments);
            Console.WriteLine("Monthly repayments of " + Math.Round(monthlyRepayments, 2));
            Console.WriteLine("Total repayments of " +  Math.Round(totalRepayments));
            return 100;
        }
       
    }
}
