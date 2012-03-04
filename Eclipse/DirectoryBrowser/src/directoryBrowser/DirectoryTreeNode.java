package directoryBrowser;

import java.io.File;

import javax.swing.tree.DefaultMutableTreeNode;

public class DirectoryTreeNode extends DefaultMutableTreeNode {
	
	class FolderNodeObject extends Object { 
		
		private File file;
		
		public FolderNodeObject(File fileObj) {
			file = fileObj;
		}
		
		public String toString() {
			String filePath = file.getPath();
			String fileName = filePath.substring(filePath.lastIndexOf('\\') + 1);
			if (fileName.isEmpty())
				fileName = filePath;
			
			if (fileName.endsWith("\\"))
				fileName = fileName.substring(0, fileName.length() - 1);
			
			return fileName;
		}
	}

	/**
	 * 
	 */
	private static final long serialVersionUID = -4252013799927069146L;
	
	public DirectoryTreeNode(File file) {
		userObject = new FolderNodeObject(file);
	}
	
	public boolean isLeaf() {
		return ((FolderNodeObject)userObject).file.isFile();
	}
	
	public File getFile() {
		return ((FolderNodeObject)userObject).file;
	}

	public void addChildNodes() {
		File fileObj = getFile();
		if (fileObj != null) {			
			if (fileObj.isDirectory()) {
				File[] subFiles = fileObj.listFiles();
				if (subFiles != null) {
					for (int i = 0; i < subFiles.length; i++) {
						if (subFiles[i].isDirectory())
							add(new DirectoryTreeNode(subFiles[i]));
					}
				}
			}
		}
	}
}
