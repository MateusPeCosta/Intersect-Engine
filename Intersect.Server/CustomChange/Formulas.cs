using System;

using Intersect.Enums;
using Intersect.Server.Entities;

using NCalc;

namespace Intersect.Server.General
{
    public partial class Formulas
    {
        public string Evasion = "((V_Evasion / 3) / A_Accuracy) * 100";

        public static int CalculateEvasion(Entity attacker, Entity victim)
        {
            if (mFormulas == null)
            {
                throw new ArgumentNullException(nameof(mFormulas));
            }

            if (attacker == null)
            {
                throw new ArgumentNullException(nameof(attacker));
            }

            if (attacker.Stat == null)
            {
                throw new ArgumentNullException(
                    nameof(attacker.Stat), $@"{nameof(attacker)}.{nameof(attacker.Stat)} is null"
                );
            }

            if (victim == null)
            {
                throw new ArgumentNullException(nameof(victim));
            }

            if (victim.Stat == null)
            {
                throw new ArgumentNullException(
                    nameof(victim.Stat), $@"{nameof(victim)}.{nameof(victim.Stat)} is null"
                );
            }

            var expression = new Expression(mFormulas.Evasion);

            if (expression.Parameters == null)
            {
                throw new ArgumentNullException(nameof(expression.Parameters));
            }

            try
            {
                expression.Parameters["A_Accuracy"] = attacker.Stat[(int)Stat.Accuracy].Value();
                expression.Parameters["V_Evasion"] = victim.Stat[(int)Stat.Evasion].Value();
                expression.EvaluateFunction += delegate (string name, FunctionArgs args)
                {
                    if (args == null)
                    {
                        throw new ArgumentNullException(nameof(args));
                    }
                };

                var result = Convert.ToDouble(expression.Evaluate());

                return (int)(Math.Round(result));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to evaluate evasion formula", ex);
            }
        }
    }
}
