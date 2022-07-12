using BusinessLogic.Models;

namespace BusinessLogic.ModelValidation
{
    public static class ModelExtensionValidation
    {
        public static bool IsModelValid(this PersonModelContext model)
        {
            if (model == null)
            {
                return false;
            }
            if ((model.Avatar == "") || (model.Avatar == null)
                || (model.Name == "") || (model.Name == null)
                || (model.Age.ToString() == "") || (model.Age.ToString() == null)
               )
            {
                return false;
            }
            return true;
        }
        public static bool IsIdValid(this string id)
        {
            int validid = 0;
            return int.TryParse(id, out validid);
        }
    }
}
