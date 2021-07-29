package be.eloistree.clipboard;
import java.io.*;
import java.util.Iterator;
import java.awt.*;
import java.awt.image.*; 
import java.awt.datatransfer.*;
import javax.imageio.*;
 
//Source: https://stackoverflow.com/questions/49761432/how-to-write-image-to-disk-from-clipboard-in-java
public class Clipboard2FileImage {

	
		    public static
		    void main(String[] args)
		    throws Exception
		    {

		        System.err.println("usage: java clipimg [filename]");
		        if(args.length>0) {
		        	for (int i = 0; i < args.length; i++) {
				        System.err.println("Arg:"+args[i]);
					}
		        	
		        }
		        System.err.println("usage: java clipimg [filename]");
		        String outputfile="default.png";
		        if(args.length==0) 
		        	loadClipboardAsFileTo(outputfile);
		        else if(args.length ==1 )
		        	outputfile=args[0];
		        else if(args.length==2) {
		        	
		        	if(args[0].toLowerCase().trim().indexOf("pull")==0) {
		        		loadClipboardAsFileTo(args[1]);
		        	}if(args[0].toLowerCase().trim().indexOf("push")==0) {
		        		loadImageToClipboard(args[1]);
		        	}
		        }
		    }
		 
		    static
		    int loadClipboardAsFileTo(String filename) throws Exception {
		        Transferable content = Toolkit.getDefaultToolkit().getSystemClipboard().getContents(null);
		        if(content==null){
		            System.err.println("error: nothing found in clipboard");
		            return 1;
		        }
		        if(!content.isDataFlavorSupported(DataFlavor.imageFlavor)){
		            System.err.println("error: no image found in clipbaord");
		            return 2;
		        }
		        BufferedImage img = (BufferedImage)content.getTransferData(DataFlavor.imageFlavor);
		        String ext = ext(filename);
		        if(ext==null){
		            ext="png";
		            filename+="."+ext;
		        }
		        File outfile=new File(filename);
		        ImageIO.write(img,ext,outfile);
		        System.err.println("image copied to: " + outfile.getAbsolutePath());
		        return 0;
		    }
		 
		 
		    public static void loadImageToClipboard (String path) {
		    	 try {
					String currentPath = new java.io.File(".").getCanonicalPath();
					File tmpDir = new File(path);
					boolean exists = tmpDir.exists();
					if(!exists)
						path= currentPath+"\\"+path;
		    	BufferedImage img = null;
		    	try {
		    	    img = ImageIO.read(new File(path));
		    	} catch (IOException e) {

			        System.err.println("Can't read image: " + path);
		    	}
		    	loadImageToClipboard(img);	
		    	} catch (IOException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
		    	 
		    }
		    
		    public static void loadImageToClipboard(Image image)
		    {
		        if (image == null)
		            throw new IllegalArgumentException ("Image can't be null");

		        ImageTransferable transferable = new ImageTransferable( image );
		        Toolkit.getDefaultToolkit().getSystemClipboard().setContents(transferable, null);
		    }
		    
		    static class ImageTransferable implements Transferable
		    {
		        private Image image;

		        public ImageTransferable (Image image)
		        {
		            this.image = image;
		        }

		        public Object getTransferData(DataFlavor flavor)
		            throws UnsupportedFlavorException
		        {
		            if (isDataFlavorSupported(flavor))
		            {
		                return image;
		            }
		            else
		            {
		                throw new UnsupportedFlavorException(flavor);
		            }
		        }

		        public boolean isDataFlavorSupported (DataFlavor flavor)
		        {
		            return flavor == DataFlavor.imageFlavor;
		        }

		        public DataFlavor[] getTransferDataFlavors ()
		        {
		            return new DataFlavor[] { DataFlavor.imageFlavor };
		        }
		    }
		    
		    static String ext(String filename){
		        int pos=filename.lastIndexOf('.')+1;
		        if(pos==0 || pos >= filename.length() )return null;
		        return filename.substring(pos);
		    }
		
}
