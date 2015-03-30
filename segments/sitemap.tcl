proc get_sitemap_data {} {
    return {
        {   
            -filename    "index.tml"
            -title       "Segments" 
            -helpurl     "http://www.lyris.com/"  
            -icon        "segments"  
        }

        {   
            -filename    "edit.tml"
            -title       "Edit Segment" 
            -hierarchy   "edit" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "edit_triggered.tml"
            -title       "Edit Triggered Segment" 
            -hierarchy   "edit_triggered" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "copy.tml"
            -title       "Copy Segment" 
            -hierarchy   "copy" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "delete.tml"
            -title       "Delete Segment" 
            -hierarchy   "delete" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "new.tml"
            -title       "New Segment" 
            -hierarchy   "new" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "new_triggered.tml"
            -title       "New Triggered Segment" 
            -hierarchy   "new_triggered" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "test.tml"
            -title       "Test Segment" 
            -hierarchy   "test" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "test_row.tml"
            -title       "View Row" 
            -hierarchy   "row" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "insert_field.tml"
            -title       "Insert Segment element" 
            -hierarchy   "edit/insert" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "insert_conditions.tml"
            -title       "Insert clause" 
            -hierarchy   "insert_conditions" 
            -helpurl     "http://www.lyris.com/"  
        }

        {   
            -filename    "insert_conditions_triggered.tml"
            -title       "Insert clause" 
            -hierarchy   "insert_conditions_triggered" 
            -helpurl     "http://www.lyris.com/"  
        }
        
                {   
            -filename    "choose_preferences.tml"
            -title       "Choose preferences" 
            -hierarchy   "choose_preferences" 
            -helpurl     "http://www.lyris.com/"  
        }
        
		{   
            -filename    "insert_trigger.tml"
            -title       "Insert trigger" 
            -hierarchy   "insert_trigger" 
            -helpurl     "http://www.lyris.com/"  
        }
    }
}

set sitemap_proc get_sitemap_data

