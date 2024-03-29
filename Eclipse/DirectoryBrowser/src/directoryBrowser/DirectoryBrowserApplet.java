package directoryBrowser;

import java.awt.*;
import java.awt.event.*;
import java.io.*;
import java.net.*;
import java.util.*;

import javax.swing.*;
import javax.swing.border.*;
import javax.swing.event.*;
import javax.swing.tree.*;

import netscape.javascript.*;

import directoryBrowser.BandFinder.*;

public class DirectoryBrowserApplet extends JApplet {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1732222036095647430L;
	
	class DirectoryTreeExpansionListener implements TreeExpansionListener {

		@Override
		public void treeCollapsed(TreeExpansionEvent arg0) {
			
		}

		@Override
		public void treeExpanded(TreeExpansionEvent arg0) {
			JTree tree = (JTree)arg0.getSource();
			TreePath selectedPath = arg0.getPath();
			
			DirectoryTreeNode selectedNode = (DirectoryTreeNode)selectedPath.getLastPathComponent();
			Enumeration<DirectoryTreeNode> childEnumeration = selectedNode.children();
			while (childEnumeration.hasMoreElements())
			{
				DirectoryTreeNode childNode = childEnumeration.nextElement();
				childNode.addChildNodes();
			}
		}
		
	}
	
	class FindBandListener implements ActionListener {
		
		JApplet _applet;
		JTree _treeFolders;
		JPanel _pnlProgress;
		JLabel _lblFindProgress;
		
		public FindBandListener(JApplet applet, JTree treeFolders, JPanel pnlProgress, JLabel lblFindProgress) {
			_applet = applet;
			_treeFolders = treeFolders;
			_pnlProgress = pnlProgress;
			_lblFindProgress = lblFindProgress;
		}
		
		public void actionPerformed(ActionEvent arg0) {				
			DirectoryTreeNode selectedNode = (DirectoryTreeNode)_treeFolders.getSelectionPath().getLastPathComponent();
			
			_pnlProgress.setVisible(true);
			_lblFindProgress.setText("Looking for bands...");
			
			BandFinder bandFinder = new BandFinder(selectedNode.getFile());
			bandFinder.addBandFoundListener(new BandFoundListener() {
				@Override
				public void onBandFound(BandFoundEvent e) {
					_lblFindProgress.setText("Band found: " + e.getBandName());
				}
			});
			bandFinder.addCompleteListener(new CompleteListener() {
				@Override
				public void onComplete(CompleteEvent e) {

					_pnlProgress.setVisible(false);
					
					StringBuffer bandsText = new StringBuffer();
					ArrayList<String> bands = e.getBands();
					for (String bandName : bands) {
						if (bandsText.length() > 0)
							bandsText.append('|');
						bandsText.append(bandName);
					}
					
					JSObject jsObj = JSObject.getWindow(_applet);
					jsObj.call("setBands", new Object[] { bandsText.toString() });
				}
			});
			
			bandFinder.execute();
		}
	}

	/**
	 * Create the applet.
	 */
	public DirectoryBrowserApplet() {
		
		DefaultMutableTreeNode rootNode = new DefaultMutableTreeNode("root");
		File[] roots = java.io.File.listRoots();
		for (int i = 0; i < roots.length; i++) {
			DirectoryTreeNode rootFolderNode = new DirectoryTreeNode(roots[i]);
			rootFolderNode.addChildNodes();
			rootNode.add(rootFolderNode);
		}
		
		getContentPane().setLayout(new GridBagLayout());
		
		JPanel pnlProgress = new JPanel();
		pnlProgress.setVisible(false);
		pnlProgress.setBorder(BorderFactory.createEtchedBorder(EtchedBorder.LOWERED));
		pnlProgress.setLayout(new BoxLayout(pnlProgress, BoxLayout.Y_AXIS));
		pnlProgress.add(Box.createRigidArea(new Dimension(0, 5)));
		JProgressBar pbarFindProgress = new JProgressBar();
		pbarFindProgress.setIndeterminate(true);
		pnlProgress.add(pbarFindProgress);
		JLabel lblFindProgress = new JLabel();
		lblFindProgress.setAlignmentX(Component.CENTER_ALIGNMENT);
		pnlProgress.add(lblFindProgress);
		pnlProgress.add(Box.createRigidArea(new Dimension(0, 5)));
		GridBagConstraints pnlProgressConstraints = new GridBagConstraints();
		pnlProgressConstraints.insets = new Insets(0, 100, 0, 100);
		pnlProgressConstraints.gridx = 0;
		pnlProgressConstraints.gridy = 0;
		pnlProgressConstraints.gridwidth = 2;
		pnlProgressConstraints.weightx = 1;
		pnlProgressConstraints.weighty = .95;
		pnlProgressConstraints.fill = GridBagConstraints.HORIZONTAL;
		getContentPane().add(pnlProgress, pnlProgressConstraints);
		
		final JTree treeFolders = new JTree(rootNode);
		treeFolders.addTreeExpansionListener(new DirectoryTreeExpansionListener());
		treeFolders.setRootVisible(false);
		treeFolders.setBorder(new TitledBorder(null, "Select a folder:", TitledBorder.LEFT, TitledBorder.TOP, null, null));
		treeFolders.setScrollsOnExpand(true);
		GridBagConstraints treeFoldersConstraints = new GridBagConstraints();
		treeFoldersConstraints.gridx = 0;
		treeFoldersConstraints.gridy = 0;
		treeFoldersConstraints.gridwidth = 2;
		treeFoldersConstraints.weightx = 1;
		treeFoldersConstraints.weighty = .95;
		treeFoldersConstraints.insets = new Insets(5, 5, 5, 5);
		treeFoldersConstraints.fill = GridBagConstraints.BOTH;
		JScrollPane scrollPane = new JScrollPane(treeFolders);
		getContentPane().add(scrollPane, treeFoldersConstraints);

		JButton btnOK = new JButton("OK");
		btnOK.addActionListener(new FindBandListener(this, treeFolders, pnlProgress, lblFindProgress));
		GridBagConstraints btnOKConstraints = new GridBagConstraints();
		btnOKConstraints.gridx = 0;
		btnOKConstraints.gridy = 1;
		btnOKConstraints.weightx = .5;
		btnOKConstraints.weighty = .05;
		btnOKConstraints.anchor = GridBagConstraints.LINE_END;
		btnOKConstraints.insets = new Insets(5, 5, 5, 5);
		getContentPane().add(btnOK, btnOKConstraints);
		
		JButton btnCancel = new JButton("Cancel");
		GridBagConstraints btnCancelConstraints = new GridBagConstraints();
		btnCancelConstraints.gridx = 1;
		btnCancelConstraints.gridy = 1;
		btnCancelConstraints.weightx = .5;
		btnCancelConstraints.weighty = .05;
		btnCancelConstraints.anchor = GridBagConstraints.LINE_START;
		btnCancelConstraints.insets = new Insets(5, 5, 5, 5);
		getContentPane().add(btnCancel, btnCancelConstraints);

	}
}
