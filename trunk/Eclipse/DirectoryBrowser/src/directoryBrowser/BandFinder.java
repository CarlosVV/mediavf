package directoryBrowser;

import java.io.*;
import java.net.MalformedURLException;
import java.util.*;
import java.util.concurrent.ExecutionException;

import javax.swing.*;
import javax.swing.event.EventListenerList;

import org.farng.mp3.*;
import org.farng.mp3.id3.*;

public class BandFinder extends SwingWorker<ArrayList<String>, String> {
	
	private final File _file;
	private EventListenerList _bandFoundListeners = new EventListenerList();
	private EventListenerList _completeListeners = new EventListenerList();
	
	public class BandFoundEvent extends EventObject {

		private static final long serialVersionUID = -415951208870046329L;
		private final String _bandName;

		public BandFoundEvent(Object source, String bandName) {
			super(source);
			_bandName = bandName;
		}
		
		public String getBandName() {
			return _bandName;
		}
	}
	
	public interface BandFoundListener extends EventListener {
		
		void onBandFound(BandFoundEvent e);
	}
	
	public class CompleteEvent extends EventObject {
		
		private static final long serialVersionUID = -7257916937389545098L;
		private final ArrayList<String> _bands;
		
		public CompleteEvent(Object source, ArrayList<String> bands) {
			super(source);
			_bands = bands;
		}
		
		public ArrayList<String> getBands() {
			return _bands;
		}
	}
	
	public interface CompleteListener extends EventListener {
		
		void onComplete(CompleteEvent e);
	}
	
	public BandFinder(File file) {
		_file = file;
	}
	
	public void addBandFoundListener(BandFoundListener listener) {
		_bandFoundListeners.add(BandFoundListener.class, listener);
	}
	
	public void removeBandFoundListener(BandFoundListener listener) {
		_bandFoundListeners.remove(BandFoundListener.class, listener);
	}
	
	public void fireBandFound(String bandName) {

		BandFoundEvent e = new BandFoundEvent(this, bandName);
		
	     Object[] listeners = _bandFoundListeners.getListenerList();
	     // loop through each listener and pass on the event if needed
	     Integer numListeners = listeners.length;
	     for (int i = 0; i<numListeners; i+=2) {
	          if (listeners[i] == BandFoundListener.class) {
	               // pass the event to the listeners event dispatch method
	                ((BandFoundListener)listeners[i+1]).onBandFound(e);
	          }            
	     }
	}
	
	public void addCompleteListener(CompleteListener listener) {
		_completeListeners.add(CompleteListener.class, listener);
	}
	
	public void removeCompleteListener(CompleteListener listener) {
		_completeListeners.add(CompleteListener.class, listener);
	}
	
	public void fireComplete(ArrayList<String> bands) {

		CompleteEvent e = new CompleteEvent(this, bands);
		
	    Object[] listeners = _completeListeners.getListenerList();
	    // loop through each listener and pass on the event if needed
	    Integer numListeners = listeners.length;
	    for (int i = 0; i<numListeners; i+=2) {
	         if (listeners[i] == CompleteListener.class) {
	              // pass the event to the listeners event dispatch method
	               ((CompleteListener)listeners[i+1]).onComplete(e);
	         }            
	    }
	}
	
	@Override
	public void done() {
		
		try {
			fireComplete(get());
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (ExecutionException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	@Override
	protected ArrayList<String> doInBackground() throws Exception {
		// TODO Auto-generated method stub
		return getBands(_file);
	}
	
	private ArrayList<String> getBands(File file) {

		ArrayList<String> bands = new ArrayList<String>();
		
		if (file.isDirectory()) {
			
			File[] files = file.listFiles(new FilenameFilter() {
				@Override
				public boolean accept(File arg0, String arg1) {
					return arg1.endsWith(".mp3");
				}
			});
			
			for (File foundFile : files) {
				
				if (foundFile.isFile()) {
					try {
						String bandName = "";
						
						MP3File mp3File = new MP3File(foundFile);
						AbstractID3v2 id3v2Tag = mp3File.getID3v2Tag();
						if (id3v2Tag != null)
							bandName = id3v2Tag.getLeadArtist();
						else {
							ID3v1 id3v1Tag = mp3File.getID3v1Tag();
							if (id3v1Tag != null)
								bandName = id3v1Tag.getArtist();
						}
						
						if (!bandName.isEmpty() && !bands.contains(bandName) && isValidBandName(bandName)) {
							fireBandFound(bandName);
							bands.add(bandName);
						}
					} catch (IOException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					} catch (TagException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
			}
			
			File[] folders = file.listFiles(new FileFilter() {

				@Override
				public boolean accept(File arg0) {
					return arg0.isDirectory();
				}
				
			});
			
			for (File folder : folders) {
				ArrayList<String> childFolderBands = getBands(folder);
				for (String bandName : childFolderBands)
					if (!bands.contains(bandName))
						bands.add(bandName);
			}
		}
		
		return bands;
	}
	
	private Boolean isValidBandName(String bandName) {
		Boolean valid = true;
		for (char c : bandName.toCharArray()) {
			if (!Character.isLetterOrDigit(c) && !Character.isWhitespace(c) &&
				c != '\'' && c != '.' && c != ':' && c != '-' && c != ';' && c != ',' && c != '!' &&
				c != '&' && c != '*' && c != '#' && c !='@' && c != '/' && c != '\\' && c != '=' &&
				c != '%' && c != '(' && c != ')' && c != '+' && c != '_' && c != '?') {
				valid = false;
				break;
			}
		}
		
		return valid;
	}
}
