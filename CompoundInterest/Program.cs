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

        public static void Main()
        {
            var path = @"C:\Users\tom.groombridge\downloads\market_no_headers.csv";
            if (File.Exists(path))
            {
                 Console.WriteLine(EnoughInPot(path));               
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", path);
            }
            
        }
        

        public static int EnoughInPot(string path)
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
                Console.WriteLine(item[2]);
                interestAvailable.Add(interestValue);
                amountAvailable.Add(decValue);
            }
            decimal totalAvailable = amountAvailable.Sum();      
      
            if (totalAvailable > 100)
            {
                var lenderAmounts = amountAvailable;
                int sum = 0;
                int i = 0;                
                do
                {
                    sum += lenderAmounts[i];
                    i++;                    
                } while (sum < 1001);

                decimal allInterest = 0;
                int x = 0;
                do
                {
                    allInterest += interestAvailable[x];
                    x++;
                } while (x < i);
                Console.WriteLine("all interest needed is " + allInterest);

                decimal compInterest = allInterest / i;
                Console.WriteLine("compound interest is " + compInterest);

                //decimal principalPayment = 27.78m;
                decimal principalPayment = (1000.00m / loanTerm);
                Console.WriteLine("The principal payment is  " + principalPayment);

                Array LendersNeeded = orderedList.Take(i).ToArray();
                SetRepayments(LendersNeeded, compInterest, principalPayment);
                return i;
            }
            else
            {
                return 0;
            }
        }

        public static int SetRepayments(Array lenders, decimal rate, decimal principal)
        {            
            decimal totalRepayments = 0;
            decimal remainingPrincipal = 1000;           
            do
            {
                decimal interestPerMonth = ((remainingPrincipal *  Math.Round(rate, 2)) / 12);
                Console.WriteLine("Interest per month " + Math.Round(interestPerMonth, 2));
                totalRepayments += (principal + Math.Round(interestPerMonth, 2));
                remainingPrincipal = (remainingPrincipal - principal);
                Console.WriteLine("Remaining Principal: " + remainingPrincipal);
            } while (remainingPrincipal >= 0);
            decimal monthlyRepayments = (totalRepayments / loanTerm);
            Console.WriteLine("total repayments " + totalRepayments);
            totalRepayments = (monthlyRepayments * loanTerm);
            Console.WriteLine("Monthly repayments of " + Math.Round(monthlyRepayments, 2));
            Console.WriteLine("Total repayments of " +  Math.Round(totalRepayments));
            return 100;
        }
       
    }
}
