using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.ValidationAttributes
{
    public class CollectionMinCountAttribut: ValidationAttribute
    {

        private readonly int _minCount;
        public CollectionMinCountAttribut(int minCount)
        {
            _minCount = minCount;
        }
        public override bool IsValid(object? value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= _minCount;
            }
            return false;
        }
    }
}
