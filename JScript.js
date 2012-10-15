// JScript File

     function wiHideShow(divId,linkId)
         {
            var obj = document.getElementById(divId);
            var obj2 = document.getElementById(linkId);
            
            if(obj.style.display == "none")
            {
                obj.style.display = "block";
                obj2.innerHTML = "Hide Work Instructions";    
            }
            else
            {
                obj.style.display = "none";
                obj2.innerHTML = "View Work Instructions";
            }
         }
         
         function validateDdl(sender, args)
         {
            if(args.Value.toString() == "select")
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
            
         }
         

         
         function chk(obj)
         {
            var obj = document.getElementById(obj.id);
            obj.value = "Checking Parts...Please wait.";
         }