namespace IM_API
{
    public static class PersonManager
    {
        public static TPERSON_V MakePersonView(TPERSON Person, TUSER_V User, TUSEROPTIONS Options, bool bIsSelf = false)
        {
            TPERSON_V result = new TPERSON_V();

            result.ID = Person.ID;
            result.DISPLAYNAME = Person.DISPLAYNAME;

            if (bIsSelf || Options.SHOW_EMAIL)
                result.EMAIL = User.EMAIL;
            if (bIsSelf || Options.SHOW_FIRSTNAME)
                result.FIRSTNAME = User.FIRSTNAME;
            if (bIsSelf || Options.SHOW_LASTNAME)
                result.LASTNAME = User.LASTNAME;
            if (bIsSelf || Options.SHOW_BIRTHDATE)
                result.BIRTHDATE = User.BIRTHDATE;
            if (bIsSelf || Options.SHOW_ROLE)
                result.ROLE = User.ROLE;

            if(bIsSelf || Options.SHOW_CREATED)
                result.CREATED = Person.CREATED;
            if(bIsSelf || Options.SHOW_CHANGED)
                result.CHANGED = Person.CHANGED;

            return result;
        }
    }
}
