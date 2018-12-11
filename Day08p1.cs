
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public class Day08p1 {
    private string inputPath;
    private int[] tokens;
    public Day08p1(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public long Solve(){
        var tree = BuildTree();
        long res = MetadataSum(tree);
        Console.WriteLine(res);
        return res;
    }

    private long MetadataSum(TreeNode node){
        long nodeMetada = 0;
        if (node.Children.Count() == 0){ 
            nodeMetada = node.Metadata.Sum();
        }
        else{
            foreach(int childIndex in node.Metadata){
                if (childIndex >= 1 && childIndex <= node.Children.Count()){
                    nodeMetada += MetadataSum(node.Children[childIndex - 1]);
                }
            }
        }
        return nodeMetada;
    }
    private TreeNode BuildTree()
    {
        int currentIndex = 0;
        return AddNode(null, ref currentIndex);
    }

    private TreeNode AddNode(TreeNode parent, ref int currentIndex)
    {
        int children = tokens[currentIndex++];
        int metadaTokens = tokens[currentIndex++];
        TreeNode node = new TreeNode();
        for(int i = 0; i < children; i++){
            var childNode = AddNode(node, ref currentIndex);
            node.AddChild(childNode);
        }
        for(int i = 0; i < metadaTokens; i++){
            node.Metadata.Add(tokens[currentIndex++]);
        }
        return node;
    }

    private void ReadInput()
    {
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){
            string s = file.ReadToEnd();
            tokens = s.Split(' ').Select(t => Int32.Parse(t)).ToArray();
            file.Close();
        }
    }
}

public class TreeNode{
    List<int> metadata;
    List<TreeNode> children;
    public TreeNode(){
        children = new List<TreeNode>();
        metadata = new List<int>();
    }

    public void AddChild(TreeNode childNode){
        children.Add(childNode);
    }

    public IList<TreeNode> Children{
        get{return children;}
    }
    public List<int> Metadata{
        get{return metadata;}
    }
}
