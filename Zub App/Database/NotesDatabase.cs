using System;
using System.Collections.Generic;
using Zub_App;
using Wintellect.Sterling;
using Wintellect.Sterling.Database;

namespace Zub_App
{
    public class NotesDatabase : BaseDatabaseInstance
    {
        public override string Name
        {
            get { return "NotesDatabase"; }
        }


        protected override List<ITableDefinition> _RegisterTables()
        {
            return new List<ITableDefinition>
            {
                    /*   {    
                           
                           CreateTableDefinition<Category, int>(i => i.categoryID),
                           CreateTableDefinition<Notes, int>(i => i.noteID)
                            .WithIndex<Notes, int, int>("noteID", x => x.noteID)
                            .WithIndex<Notes, int, int>("categoryID", notes => notes.categoryID ),
                       };
                    */

            
                           CreateTableDefinition<Category, int>(fg => fg.Id)
                                .WithIndex<Category, int, int>("categoryID", category => category.categoryID ),
							CreateTableDefinition<Notes, int>(fg => fg.Id)
							   .WithIndex<Notes, string, int>("NotesName", notes => notes.noteName )
                               .WithIndex<Notes, int, int>("CategoryID", notes => notes.categoryID ),
            };  
        }
    }
}
