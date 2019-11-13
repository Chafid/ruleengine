using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using static RuleEngine.Program;

namespace RuleEngine
{
    static class Constants
    {
        public const string PromoCap = "50000";
        public const string StartDate = "10/10/2019";
        public const string EndDate = "17/10/2019";
    }
    public class Program
    {
        public class Promo
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int PromoCap { get; set; }
            public string PromoName { get; set; }

        }

        public class Transaction
        {
            public DateTime TransactionDate { get; set; }
            public int Amount { get; set; }

        }
        
        public static int CompileRule<T>(Rule r)
        {
            var promoParam = Expression.Parameter(typeof(Promo));
            Expression expr = BuildExpr<T>(r, promoParam);

            return Expression.Lambda<int>(expr, promoParam).Compile();
        }

        public static Expression BuildExpr<T>(Rule r, ParameterExpression param)
        {
            var left = MemberExpression.Property(param, r.MemberName);
            var tProp = typeof(T).GetProperty(r.MemberName).PropertyType;
            ExpressionType tBinary;
            if (ExpressionType.TryParse(r.Operator, out tBinary))
            {
                var right = Expression.Constant(Convert.ChangeType(r.TargetValue, tProp);
                return Expression.MakeBinary(tBinary, left, right);
            }
            else
            {
                var method = tProp.GetMethod(r.Operator);
                var tParam = method.GetParameters()[0].ParameterType;
                var right = Expression.Constant(Convert.ChangeType(r.TargetValue, tParam));

                return Expression.Call(left, method, right);
            }
        }


        public class Rule
        {
            public string MemberName { get; set; }
            public string Operator { get; set; }
            public string TargetValue { get; set; }
            public int Discount { get; set; }
            public Rule(string MemberName, string Operator, string TargetValue)
            {
                this.MemberName = MemberName;
                this.Operator = Operator;
                this.TargetValue = TargetValue;
                this.Discount = Discount;
            }

        }

        static void Main()
        {
            List<Rule> rules = new List<Rule>
            {
                new Rule ("StartDate", "GreaterThan", "20/10/2019"),
                new Rule ("PromoCap", "LessThan", Constants.PromoCap)

            };

            var Transaction1 = new Transaction
            {
                TransactionDate = Convert.ToDateTime("12/10/2019"),
                Amount = 40000
            };

            Console.WriteLine("Hello World!");
        }
    }
}
