namespace IM_API
{
    public static class PersonManager
    {
        public static TPERSON_V MakePersonView(TPERSON Person, TUSER_V User, TUSEROPTIONS Options)
        {
            TPERSON_V result = new TPERSON_V();

            result.ID = Person.ID;
            result.DISPLAYNAME = Person.DISPLAYNAME;

            if (Options.SHOW_EMAIL)
                result.EMAIL = User.EMAIL;
            if (Options.SHOW_FIRSTNAME)
                result.FIRSTNAME = User.FIRSTNAME;
            if (Options.SHOW_LASTNAME)
                result.LASTNAME = User.LASTNAME;
            if (Options.SHOW_BIRTHDATE)
                result.BIRTHDATE = User.BIRTHDATE;
            if (Options.SHOW_ROLE)
                result.ROLE = User.ROLE;

            if(Options.SHOW_CREATED)
                result.CREATED = Person.CREATED;
            if(Options.SHOW_CHANGED)
                result.CHANGED = Person.CHANGED;

            return result;
        }
    }
}
